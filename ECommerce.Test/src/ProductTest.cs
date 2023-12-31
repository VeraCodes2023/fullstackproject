using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using ECommerce.Business;
using  Core;

namespace ECommerceTest;
public class ProductTest
{
    private static IMapper _mapper;

    public ProductTest()
    {
        if (_mapper == null)
        {
            var mappingConfig = new MapperConfiguration(m =>
            {
                m.AddProfile(new MapperProfile());
            });
            
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
        }
    }

    [Fact]
    
    public void CreateProduct_ValidProduct_ReturnsProductReadDTO()
    {
        var mockMapper = new Mock<IMapper>();
        var mockProductRepo = new Mock<IProductRepo>();
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object);

        var productCreateDto = new ProductCreateDTO
        {
            Title= "Suomen Mestari 6",
            Price= 40,
            Description= "Language learning level C2",
            Inventory=200,
            CategoryId= 1,
            Images = new List<ImageCreateDTO> { new ImageCreateDTO { Url = "https://fakeimg.pl/200x200" } }
        };
        var newProduct = new Product(); 
        var createdProduct = new Product();
        var productReadDto = new ProductReadDTO();

        mockMapper.Setup(mapper => mapper.Map<ProductCreateDTO, Product>(productCreateDto)).Returns(newProduct);
        mockMapper.Setup(mapper => mapper.Map<ImageCreateDTO, Image>(It.IsAny<ImageCreateDTO>())).Returns(new Image()); 
        mockProductRepo.Setup(repo => repo.Create(newProduct)).Returns(createdProduct);
        mockMapper.Setup(mapper => mapper.Map<Product, ProductReadDTO>(createdProduct)).Returns(productReadDto); 

        var result = productService.CreateProduct(productCreateDto);
        Assert.NotNull(result);
    }

    [Fact]
    public void CreateProduct_NullProduct_ThrowsException()
    {
     
        var mockMapper = new Mock<IMapper>();
        var mockProductRepo = new Mock<IProductRepo>();
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object);
        ProductCreateDTO productCreateDto = null;
        Assert.Throws<Exception>(() => productService.CreateProduct(productCreateDto));
    }

    [Fact]
    public void DeleteProduct_ValidProductId_DeletesProductAndReturnsTrue()
    {
        var productId = Guid.NewGuid(); 

        var mockMapper = new Mock<IMapper>();
        var mockProductRepo = new Mock<IProductRepo>();
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object);

        var targetProduct = new Product { Id = productId }; 

        mockProductRepo.Setup(repo => repo.GetById(productId)).Returns(targetProduct);
        var result = productService.DeleteProduct(productId);

        Assert.True(result);
        mockProductRepo.Verify(repo => repo.Delete(productId), Times.Once); 
    }

    [Fact]
    public void DeleteProduct_EmptyProductId_ThrowsException()
    {
        var productId = Guid.Empty;

        var mockMapper = new Mock<IMapper>();
        var mockProductRepo = new Mock<IProductRepo>();
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object);

        Assert.Throws<Exception>(() => productService.DeleteProduct(productId));
        mockProductRepo.Verify(repo => repo.Delete(It.IsAny<Guid>()), Times.Never); 
    }

    [Fact]
    public void DeleteProduct_NonexistentProductId_ReturnsFalse()
    {
        var productId = Guid.NewGuid(); // invalid id

        var mockMapper = new Mock<IMapper>();
        var mockProductRepo = new Mock<IProductRepo>();
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object);

        mockProductRepo.Setup(repo => repo.GetById(productId)).Returns(() => null);

        var result = productService.DeleteProduct(productId!);
        Assert.False(result);
        mockProductRepo.Verify(repo => repo.Delete(It.IsAny<Guid>()), Times.Never);
    }


    [Fact]
    public void GetAllProducts_ReturnsMappedProductReadDTOs()
    {
        var options = new ProductQueryParameters();
        options.Limit = 2;
        options.Offset = 0;

        var products = new List<Product>
        {
            new Product {
                Title = "Suomen Mestari 5",
                Price = 35,
                Description = "Language learning level C2",
                Inventory = 100,
                CategoryId = 1
            },
            new Product { 
                Title = "Suomen Mestari 6",
                Price = 45,
                Description = "Language learning level C1",
                Inventory = 200,
                CategoryId = 1
            }
        };

        var mappedProductReadDTOs = new List<ProductReadDTO>
        {
            new ProductReadDTO { 
                Title = "Suomen Mestari 6",
                Price = 45,
                Description = "Language learning level C1",
                Inventory = 200
            },
            new ProductReadDTO { 
                Title = "Suomen Mestari 5",
                Price = 35,
                Description = "Language learning level C2",
                Inventory = 100
            }
        };

        var mockProductRepo = new Mock<IProductRepo>();
        var mockMapper = new Mock<IMapper>();
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object);

        mockProductRepo.Setup(repo => repo.GetAll(options)).Returns(products.AsQueryable());
        mockMapper.Setup(mapper => mapper.Map<Product, ProductReadDTO>(It.IsAny<Product>()))
            .Returns((Product p) => mappedProductReadDTOs.FirstOrDefault(dto => dto.Title == p.Title)); 

        var result = productService.GetAllProducts(options);

        Assert.NotNull(result);
        Assert.Equal(mappedProductReadDTOs.Count, result.Count());

    }

    [Fact]
    public void GetProductById_ValidId_ReturnsMappedProductReadDTO()
    {
        var productId = Guid.NewGuid(); // valid id

        var mockProductRepo = new Mock<IProductRepo>();
        var mockMapper = new Mock<IMapper>();
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object);

        var targetProduct = new Product { Id = productId, };
        var mappedResult = new ProductReadDTO {  };
        var imageReadDTOs = new List<ImageReadDTO> {  };

        mockProductRepo.Setup(repo => repo.GetById(productId)).Returns(targetProduct);
        mockMapper.Setup(mapper => mapper.Map<ProductReadDTO>(targetProduct)).Returns(mappedResult);
        mockMapper.Setup(mapper => mapper.Map<List<ImageReadDTO>>(targetProduct.Images)).Returns(imageReadDTOs);

        var result = productService.GetProductById(productId);
        Assert.NotNull(result);
    }

    [Fact]
    public void GetProductById_EmptyId_ThrowsException()
    {

        var productId = Guid.Empty; 

        var mockProductRepo = new Mock<IProductRepo>();
        var mockMapper = new Mock<IMapper>();
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object);
        Assert.Throws<Exception>(() => productService.GetProductById(productId));
    }

    [Fact]
    public void GetProductById_NonexistentId_ThrowsException()
    {
        var productId = Guid.NewGuid(); 

        var mockProductRepo = new Mock<IProductRepo>();
        var mockMapper = new Mock<IMapper>();
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object);

        mockProductRepo.Setup(repo => repo.GetById(productId)).Returns(() => null);
        Assert.Throws<Exception>(() => productService.GetProductById(productId));
    }

    // [Fact]
    // public void UpdateProduct_ValidIdAndProduct_ReturnsMappedProductReadDTO()
    // {
    //     var mockProductRepo = new Mock<IProductRepo>();
    //     var mockMapper = new Mock<IMapper>();
    //     var productService = new ProductService(mockProductRepo.Object, mockMapper.Object);

    //     var productId = Guid.NewGuid(); 
    //     var productUpdateDto = new ProductUpdateDTO {Title="update book1", Price=12, Inventory=1000,Description="description"};

    //     var targetProduct = new Product { Id = productId, Title = productUpdateDto.Title, Price = productUpdateDto.Price, Inventory = productUpdateDto.Inventory, Description = productUpdateDto.Description };
    //     var mappedResult = new ProductReadDTO { Title = "update book1", Price = 12, Inventory = 1000, Description = "description"};

    //     mockProductRepo.Setup(repo => repo.GetById(productId)).Returns(targetProduct);
    //     // mockMapper.Setup(mapper => mapper.Map<List<Image>>(productUpdateDto.Image)).Returns(new List<Image> {});
    //     mockMapper.Setup(mapper => mapper.Map<ProductReadDTO>(targetProduct)).Returns(mappedResult);

    //     var result = productService.UpdateProduct(productId, productUpdateDto);
    //     Assert.NotNull(result);
    // }

    [Fact]
    public void UpdateProduct_EmptyId_ThrowsException()
    {
        var productId = Guid.Empty; 
        var productUpdateDto = new ProductUpdateDTO { };

        var mockProductRepo = new Mock<IProductRepo>();
        var mockMapper = new Mock<IMapper>();
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object);

        Assert.Throws<Exception>(() => productService.UpdateProduct(productId, productUpdateDto));
    }

    [Fact]
    public void UpdateProduct_NonexistentId_ThrowsException()
    {
        var productId = Guid.NewGuid(); 
        var productUpdateDto = new ProductUpdateDTO {  };

        var mockProductRepo = new Mock<IProductRepo>();
        var mockMapper = new Mock<IMapper>();
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object);
        mockProductRepo.Setup(repo => repo.GetById(productId)).Returns(() => null);
        Assert.Throws<Exception>(() => productService.UpdateProduct(productId, productUpdateDto));
    }



}

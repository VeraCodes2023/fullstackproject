using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;


namespace ECommerceTest;

public class ProductTest
{
    [Fact]
    public void CreateProduct_ValidProduct_ReturnsProductReadDTO()
    {
        var productCreateDto = new ProductCreateDTO
        {
        
        };

        var mockMapper = new Mock<IMapper>();
        var mockProductRepo = new Mock<IProductRepo>();
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object, /* pass other dependencies */);

        var newProduct = new Product {  };
        var createdProduct = new Product { };

        mockMapper.Setup(mapper => mapper.Map<ProductCreateDTO, Product>(productCreateDto)).Returns(newProduct);
        mockMapper.Setup(mapper => mapper.Map<ImageCreateDTO, Image>(It.IsAny<ImageCreateDTO>())).Returns(new Image()); 
        mockProductRepo.Setup(repo => repo.Create(newProduct)).Returns(createdProduct);
        mockMapper.Setup(mapper => mapper.Map<Product, ProductReadDTO>(createdProduct)).Returns(new ProductReadDTO {  });

        var result = productService.CreateProduct(productCreateDto);
        Assert.NotNull(result);
    }

    [Fact]
    public void CreateProduct_NullProduct_ThrowsException()
    {
        ProductCreateDTO productCreateDto = null;

        var mockMapper = new Mock<IMapper>();
        var mockProductRepo = new Mock<IProductRepo>();
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object);

        Assert.Throws<Exception>(() => productService.CreateProduct(productCreateDto));
    }
     [Fact]
    public void DeleteProduct_ValidProductId_DeletesProductAndReturnsTrue()
    {
        var productId = Guid.NewGuid(); // valid id 

        var mockProductRepo = new Mock<IProductRepo>();
        var productService = new ProductService(mockProductRepo.Object, /* pass other dependencies */);

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

        var mockProductRepo = new Mock<IProductRepo>();
        var productService = new ProductService(mockProductRepo.Object, /* pass other dependencies */);

        Assert.Throws<Exception>(() => productService.DeleteProduct(productId));
        mockProductRepo.Verify(repo => repo.Delete(It.IsAny<Guid>()), Times.Never); 
    }

    [Fact]
    public void DeleteProduct_NonexistentProductId_ReturnsFalse()
    {
        var productId = Guid.NewGuid(); // invalid id

        var mockProductRepo = new Mock<IProductRepo>();
        var productService = new ProductService(mockProductRepo.Object, /* pass other dependencies */);

        mockProductRepo.Setup(repo => repo.GetById(productId)).Returns(() => null);

        var result = productService.DeleteProduct(productId);
        Assert.False(result);
        mockProductRepo.Verify(repo => repo.Delete(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void GetAllProducts_ReturnsMappedProductReadDTOs()
    {
        var options = new ProductQueryParameters { /* valid params */ };
        var products = new List<Product>
        {
            new Product {  },
            new Product {  },
        };

        var mappedProductReadDTOs = new List<ProductReadDTO>
        {
            new ProductReadDTO {  },
            new ProductReadDTO {  },
        };

        var mockProductRepo = new Mock<IProductRepo>();
        var mockMapper = new Mock<IMapper>();
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object, /* pass other dependencies */);

        mockProductRepo.Setup(repo => repo.GetAll(options)).Returns(products.AsQueryable());
        mockMapper.Setup(mapper => mapper.Map<Product, ProductReadDTO>(It.IsAny<Product>()))
            .Returns((Product p) => mappedProductReadDTOs.FirstOrDefault(dto => /* match criteria for mapping */));

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
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object, /* pass other dependencies */);

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
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object, /* pass other dependencies */);
        Assert.Throws<Exception>(() => productService.GetProductById(productId));
    }

    [Fact]
    public void GetProductById_NonexistentId_ThrowsException()
    {
        var productId = Guid.NewGuid(); 

        var mockProductRepo = new Mock<IProductRepo>();
        var mockMapper = new Mock<IMapper>();
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object, /* pass other dependencies */);

        mockProductRepo.Setup(repo => repo.GetById(productId)).Returns(() => null);
        Assert.Throws<Exception>(() => productService.GetProductById(productId));
    }
      [Fact]
    public void UpdateProduct_ValidIdAndProduct_ReturnsMappedProductReadDTO()
    {
        var productId = Guid.NewGuid(); 
        var productUpdateDto = new ProductUpdateDTO {  };

        var mockProductRepo = new Mock<IProductRepo>();
        var mockMapper = new Mock<IMapper>();
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object, /* pass other dependencies */);

        var targetProduct = new Product { Id = productId,  };
        var mappedResult = new ProductReadDTO { };

        mockProductRepo.Setup(repo => repo.GetById(productId)).Returns(targetProduct);
        mockMapper.Setup(mapper => mapper.Map<List<Image>>(productUpdateDto.ImageUpdateDTOs)).Returns(new List<Image> { /* mocked Image list*/ });
        mockMapper.Setup(mapper => mapper.Map<ProductReadDTO>(targetProduct)).Returns(mappedResult);

        var result = productService.UpdateProduct(productId, productUpdateDto);
        Assert.NotNull(result);
    }

    [Fact]
    public void UpdateProduct_EmptyId_ThrowsException()
    {
        var productId = Guid.Empty; 
        var productUpdateDto = new ProductUpdateDTO { };

        var mockProductRepo = new Mock<IProductRepo>();
        var mockMapper = new Mock<IMapper>();
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object, /* pass other dependencies */);

        Assert.Throws<Exception>(() => productService.UpdateProduct(productId, productUpdateDto));
    }

    [Fact]
    public void UpdateProduct_NonexistentId_ThrowsException()
    {
        var productId = Guid.NewGuid(); // valid
        var productUpdateDto = new ProductUpdateDTO {  };

        var mockProductRepo = new Mock<IProductRepo>();
        var mockMapper = new Mock<IMapper>();
        var productService = new ProductService(mockProductRepo.Object, mockMapper.Object, /* pass other dependencies */);
        mockProductRepo.Setup(repo => repo.GetById(productId)).Returns(() => null);
        Assert.Throws<Exception>(() => productService.UpdateProduct(productId, productUpdateDto));
    }





}

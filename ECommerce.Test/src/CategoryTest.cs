using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Core;
using Moq;
using AutoMapper;
using ECommerce.Business;


namespace ECommerceTest;
public class CategoryTest
{
    private  IMapper _mapper;

    public CategoryTest()
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
    public void CreateNew_ValidCategory_ReturnsMappedCategoryReadDTO()
    {

        var mockRepo = new Mock<ICategoryRepo>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService(mockRepo.Object, mockMapper.Object);

        var categoryCreateDTO = new CategoryCreateDTO { Name = "Hnm", Image = "image1" };
        var newCategory = new Category(); 
        var mappedResult = new CategoryReadDTO(); 

        mockMapper.Setup(mapper => mapper.Map<CategoryCreateDTO, Category>(categoryCreateDTO)).Returns(newCategory);
        mockRepo.Setup(repo => repo.CreateNew(newCategory)).Returns(newCategory);
        mockMapper.Setup(mapper => mapper.Map<Category, CategoryReadDTO>(newCategory)).Returns(mappedResult);

        var result = categoryService.CreateNew(categoryCreateDTO);
        Assert.NotNull(result);
    }

    [Fact]
    public void CreateNew_NullCategory_ThrowsException()
    {
        CategoryCreateDTO categoryCreateDto = null;

        var mockRepo = new Mock<ICategoryRepo>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService(mockRepo.Object, mockMapper.Object);

        Assert.Throws<Exception>(() => categoryService.CreateNew(categoryCreateDto));
    }

    [Fact]
    public void Delete_ValidId_DeletesCategoryAndReturnsTrue()
    {

        var categoryId = 1;

        var mockRepo = new Mock<ICategoryRepo>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService(mockRepo.Object, mockMapper.Object);

        var targetCategory = new Category { Id = categoryId }; 
        mockRepo.Setup(repo => repo.GetById(categoryId)).Returns(targetCategory);
        var result = categoryService.Delete(categoryId);
        Assert.True(result);
        mockRepo.Verify(repo => repo.Delete(categoryId), Times.Once);
    }

    // [Fact]
    // public void Delete_EmptyId_ThrowsException()
    // {
    //     var categoryId =12120; 
      
    //     var mockRepo = new Mock<ICategoryRepo>();
    //     var mockMapper = new Mock<IMapper>();
    //     var categoryService = new CategoryService(mockRepo.Object, mockMapper.Object);

    //     Assert.Throws<Exception>(() => categoryService.Delete(categoryId));
    //     mockRepo.Verify(repo => repo.Delete(categoryId), Times.Once);
    // }

    [Fact]
    public void Delete_NonexistentId_ReturnsFalse()
    {
        var categoryId = 100110; 
   
        var mockRepo = new Mock<ICategoryRepo>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService(mockRepo.Object, mockMapper.Object);

        mockRepo.Setup(repo => repo.GetById(categoryId)).Returns(() => null);
        var result = categoryService.Delete(categoryId);
        Assert.False(result);
        mockRepo.Verify(repo => repo.Delete(categoryId), Times.Never); 
    }

    [Fact]
    public void GetAll_ReturnsMappedCategoryReadDTOs()
    {
        var categories = new List<Category>
        {
            new Category {Name="Interior Decoration1",Image="www.fakeiange1.com"},
            new Category {Name="Interior Decoration2",Image="www.fakeiange2.com"}
        };

        var mappedCategoryReadDTOs = new List<CategoryReadDTO>
        {
            new CategoryReadDTO {Name="Interior Decoration1",Image="www.fakeiange1.com"},
            new CategoryReadDTO {Name="Interior Decoration2",Image="www.fakeiange2.com"},
        };

        var mockRepo = new Mock<ICategoryRepo>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService(mockRepo.Object, mockMapper.Object);

        mockRepo.Setup(repo => repo.GetAll()).Returns(categories.AsQueryable());
        mockMapper.Setup(mapper => mapper.Map<Category, CategoryReadDTO>(It.IsAny<Category>()))
          .Returns((Category c) => mappedCategoryReadDTOs.FirstOrDefault(dto => dto.Name == c.Name && dto.Image == c.Image));

        var result = categoryService.GetAll();
        Assert.NotNull(result);
        Assert.Equal(mappedCategoryReadDTOs.Count, result.Count()); 
    }


    [Fact]
    public void GetById_ValidId_ReturnsMappedCategoryReadDTO()
    {
        var categoryId = 1; 

        var mockRepo = new Mock<ICategoryRepo>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService(mockRepo.Object, mockMapper.Object);

        var targetCategory = new Category { };
        var mappedResult = new CategoryReadDTO { };
        mockRepo.Setup(repo => repo.GetById(categoryId)).Returns(targetCategory);
        mockMapper.Setup(mapper => mapper.Map<CategoryReadDTO>(targetCategory)).Returns(mappedResult);
        var result = categoryService.GetById(categoryId);
        Assert.NotNull(result);
    }

    [Fact]
    public void GetById_EmptyId_ThrowsException()
    { 
        var categoryId = 0; 

        var mockRepo = new Mock<ICategoryRepo>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService(mockRepo.Object, mockMapper.Object);

        Assert.Throws<Exception>(() => categoryService.GetById(categoryId));
    }

    [Fact]
    public void GetById_NonexistentId_ThrowsException()
    {
        var categoryId = 0; 

        var mockRepo = new Mock<ICategoryRepo>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService(mockRepo.Object, mockMapper.Object);

        mockRepo.Setup(repo => repo.GetById(categoryId)).Returns(() => null);
        Assert.Throws<Exception>(() => categoryService.GetById(categoryId));
    }

     [Fact]
    public void Update_ValidIdAndCategory_ReturnsMappedCategoryReadDTO()
    {
        var categoryId = 2; 
        var categoryUpdateDto = new CategoryUpdateDTO {  };

        var mockRepo = new Mock<ICategoryRepo>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService(mockRepo.Object, mockMapper.Object);

        var targetCategory = new Category { Id = categoryId,  Name="Interior Decoration1",Image="www.fakeiange1.com"};
        var mappedResult = new CategoryReadDTO {Name="Interior Decoration1",Image="www.fakeiange1.com"  };
        mockRepo.Setup(repo => repo.GetById(categoryId)).Returns(targetCategory);
        mockMapper.Setup(mapper => mapper.Map<CategoryReadDTO>(targetCategory)).Returns(mappedResult);
        var result = categoryService.Update(categoryId, categoryUpdateDto);
        Assert.NotNull(result);
    }

    [Fact]
    public void Update_EmptyId_ThrowsException()
    {
        var categoryId = 0; 
        var categoryUpdateDto = new CategoryUpdateDTO {Name="Interior Decoration1",Image="www.fakeiange1.com"};

        var mockRepo = new Mock<ICategoryRepo>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService(mockRepo.Object, mockMapper.Object);

        Assert.Throws<Exception>(() => categoryService.Update(categoryId, categoryUpdateDto));
    }

    [Fact]
    public void Update_NonexistentId_ThrowsException()
    {
        var categoryId = 0;
        var categoryUpdateDto = new CategoryUpdateDTO {Name="Interior Decoration1",Image="www.fakeiange1.com"};

        var mockRepo = new Mock<ICategoryRepo>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService(mockRepo.Object, mockMapper.Object);

        mockRepo.Setup(repo => repo.GetById(categoryId)).Returns(() => null);
        Assert.Throws<Exception>(() => categoryService.Update(categoryId, categoryUpdateDto));
    }

}


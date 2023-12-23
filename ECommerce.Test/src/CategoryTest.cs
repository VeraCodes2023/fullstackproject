using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ECommerceTest;

public class CategoryTest
{
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
    
        var categoryCreateDto = new CategoryCreateDTO(Name:"Plant Seeds",Image:"https://fakeimg.pl/200x200")

        var mockRepository = new Mock<ICategoryRepository>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService();
        var newCategory = new Category {  };
        var mappedResult = new CategoryReadDTO { };

        mockMapper.Setup(mapper => mapper.Map<CategoryCreateDTO, Category>(categoryCreateDto)).Returns(newCategory);
        mockRepository.Setup(repo => repo.CreateNew(newCategory)).Returns(newCategory);
        mockMapper.Setup(mapper => mapper.Map<Category, CategoryReadDTO>(newCategory)).Returns(mappedResult);

        var result = categoryService.CreateNew(categoryCreateDto);
        Assert.NotNull(result);
    }

    [Fact]
    public void CreateNew_NullCategory_ThrowsException()
    {
        CategoryCreateDTO categoryCreateDto = null;

        var mockRepository = new Mock<ICategoryRepository>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService(mockRepository.Object, mockMapper.Object, /* pass other dependencies */);
        Assert.Throws<Exception>(() => categoryService.CreateNew(categoryCreateDto));
    }

    [Fact]
    public void Delete_ValidId_DeletesCategoryAndReturnsTrue()
    {

        var categoryId = Guid.NewGuid();

        var mockRepository = new Mock<ICategoryRepository>();
        var categoryService = new CategoryService(mockRepository.Object, /* pass other dependencies */);

        var targetCategory = new Category { Id = categoryId }; 
        mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(targetCategory);
        var result = categoryService.Delete(categoryId);
        Assert.True(result);
        mockRepository.Verify(repo => repo.Delete(categoryId), Times.Once);
    }

    [Fact]
    public void Delete_EmptyId_ThrowsException()
    {
        var categoryId = Guid.Empty; 
        var mockRepository = new Mock<ICategoryRepository>();
        var categoryService = new CategoryService(mockRepository.Object, /* pass other dependencies */);
        Assert.Throws<Exception>(() => categoryService.Delete(categoryId));
        mockRepository.Verify(repo => repo.Delete(It.IsAny<Guid>()), Times.Never); 
    }

    [Fact]
    public void Delete_NonexistentId_ReturnsFalse()
    {
        var categoryId = Guid.NewGuid(); 
        var mockRepository = new Mock<ICategoryRepository>();
        var categoryService = new CategoryService(mockRepository.Object, /* pass other dependencies */);

        mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(() => null);
        var result = categoryService.Delete(categoryId);
        Assert.False(result);
        mockRepository.Verify(repo => repo.Delete(It.IsAny<Guid>()), Times.Never); 
    }

    [Fact]
    public void GetAll_ReturnsMappedCategoryReadDTOs()
    {
        var categories = new List<Category>
        {
            new Category { },
            new Category { },
        };

        var mappedCategoryReadDTOs = new List<CategoryReadDTO>
        {
            new CategoryReadDTO { },
            new CategoryReadDTO { },
        };

        var mockRepository = new Mock<ICategoryRepository>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService(mockRepository.Object, mockMapper.Object, /* pass other dependencies */);
        mockRepository.Setup(repo => repo.GetAll()).Returns(categories.AsQueryable());
        mockMapper.Setup(mapper => mapper.Map<Category, CategoryReadDTO>(It.IsAny<Category>()))
            .Returns((Category c) => mappedCategoryReadDTOs.FirstOrDefault(dto => /* match criteria for mapping */));

        var result = categoryService.GetAll();
        Assert.NotNull(result);
        Assert.Equal(mappedCategoryReadDTOs.Count, result.Count()); 
    }

    [Fact]
    public void GetById_ValidId_ReturnsMappedCategoryReadDTO()
    {
        var categoryId = Guid.NewGuid(); 

        var mockRepository = new Mock<ICategoryRepository>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService(mockRepository.Object, mockMapper.Object, /* pass other dependencies */);

        var targetCategory = new Category { };
        var mappedResult = new CategoryReadDTO { };
        mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(targetCategory);
        mockMapper.Setup(mapper => mapper.Map<CategoryReadDTO>(targetCategory)).Returns(mappedResult);
        var result = categoryService.GetById(categoryId);
        Assert.NotNull(result);
    }

    [Fact]
    public void GetById_EmptyId_ThrowsException()
    { 
        var categoryId = Guid.Empty; 
        var mockRepository = new Mock<ICategoryRepository>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService(mockRepository.Object, mockMapper.Object, /* pass other dependencies */);
        Assert.Throws<Exception>(() => categoryService.GetById(categoryId));
    }

    [Fact]
    public void GetById_NonexistentId_ThrowsException()
    {
        var categoryId = Guid.NewGuid(); 

        var mockRepository = new Mock<ICategoryRepository>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService(mockRepository.Object, mockMapper.Object, /* pass other dependencies */);
        mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(() => null);
        Assert.Throws<Exception>(() => categoryService.GetById(categoryId));
    }

    [Fact]
    public void Update_ValidIdAndCategory_ReturnsMappedCategoryReadDTO()
    {
        var categoryId = Guid.NewGuid(); // vslid id
        var categoryUpdateDto = new CategoryUpdateDTO {  };

        var mockRepository = new Mock<ICategoryRepository>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService(mockRepository.Object, mockMapper.Object, /* pass other dependencies */);

        var targetCategory = new Category { Id = categoryId,  };
        var mappedResult = new CategoryReadDTO {  };
        mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(targetCategory);
        mockMapper.Setup(mapper => mapper.Map<CategoryReadDTO>(targetCategory)).Returns(mappedResult);
        var result = categoryService.Update(categoryId, categoryUpdateDto);
        Assert.NotNull(result);
    }

    [Fact]
    public void Update_EmptyId_ThrowsException()
    {
        var categoryId = Guid.Empty; 
        var categoryUpdateDto = new CategoryUpdateDTO { };

        var mockRepository = new Mock<ICategoryRepository>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService(mockRepository.Object, mockMapper.Object, /* pass other dependencies */);
        Assert.Throws<Exception>(() => categoryService.Update(categoryId, categoryUpdateDto));
    }

    [Fact]
    public void Update_NonexistentId_ThrowsException()
    {
        var categoryId = Guid.NewGuid();
        var categoryUpdateDto = new CategoryUpdateDTO { };

        var mockRepository = new Mock<ICategoryRepository>();
        var mockMapper = new Mock<IMapper>();
        var categoryService = new CategoryService(mockRepository.Object, mockMapper.Object, /* pass other dependencies */);
        mockRepository.Setup(repo => repo.GetById(categoryId)).Returns(() => null);
        Assert.Throws<Exception>(() => categoryService.Update(categoryId, categoryUpdateDto));
    }

}

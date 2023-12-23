using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core;

namespace ECommerceBusiness;
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepo _repository;
    private IMapper _mapper;

    public CategoryService(ICategoryRepo repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;

    }

    public CategoryReadDTO CreateNew(CategoryCreateDTO category)
    {
        if(category is null)
        {
            throw new Exception("bad request");
        }
        var newCategory = _mapper.Map<CategoryCreateDTO, Category>(category);
        var result = _repository.CreateNew(newCategory);
        return _mapper.Map<Category, CategoryReadDTO>(result);
    }

    public bool Delete(Guid id)
    {
        if(id==Guid.Empty)
        {
           throw new Exception("bad request");
        }
        var targetCategory= _repository.GetById(id);
        if(targetCategory is not null)
        {
            _repository.Delete(id);
            return true;
        }
        return false;
    }

    public IEnumerable<CategoryReadDTO> GetAll()
    {
        return _repository.GetAll().Select(c=>_mapper.Map<Category,CategoryReadDTO>(c));
    }

    public CategoryReadDTO GetById(Guid id)
    {
         if(id == Guid.Empty)
        {
            throw new Exception("bad request");
        }
        try
        {
            var targetCategory =  _repository.GetById(id);
            if(targetCategory  is not null)
            {
                var mappedResult = _mapper.Map<CategoryReadDTO>(targetCategory);
                return mappedResult;
            }
            throw new Exception("not found");
        }
        catch(Exception)
        {
           throw;
        }
    }

    public CategoryReadDTO Update(Guid id, CategoryUpdateDTO category)
    {
        if(id == Guid.Empty)
        {
           throw new Exception("bad request");
        }
        try
        {
            var targetCategory = _repository.GetById(id);
            if(targetCategory is not null)
            {
                
                targetCategory!.Name = category.Name;
                targetCategory!.Image = category.Image;
                _repository.Update(id,targetCategory);
                var mappedResult = _mapper.Map<CategoryReadDTO>(targetCategory);
                return _mapper.Map<CategoryReadDTO>(targetCategory);
        }
            throw new Exception("not found");
        }
        catch(Exception)
        {
            throw;
        }
    }
}

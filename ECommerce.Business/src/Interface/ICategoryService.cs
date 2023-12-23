using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceBusiness;
public interface ICategoryService
{
    IEnumerable<CategoryReadDTO> GetAll();
    CategoryReadDTO CreateNew(CategoryCreateDTO category);// admin auth
    CategoryReadDTO GetById(Guid id); // admin auth
    CategoryReadDTO Update(Guid id, CategoryUpdateDTO category); // admin auth
    bool Delete(Guid id); // admin auth
}

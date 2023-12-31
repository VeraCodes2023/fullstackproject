using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Business;
public interface ICategoryService
{
    IEnumerable<CategoryReadDTO> GetAll();
    CategoryReadDTO CreateNew(CategoryCreateDTO category);// admin auth
    CategoryReadDTO GetById(int id); // admin auth
    CategoryReadDTO Update(int id, CategoryUpdateDTO category); // admin auth
    bool Delete(int id); // admin auth
}

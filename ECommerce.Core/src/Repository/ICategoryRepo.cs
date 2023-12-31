using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core;
public interface ICategoryRepo
{
    IEnumerable<Category> GetAll();
    Category CreateNew(Category category);// admin auth
    Category GetById(int id); // admin auth
    Category Update(int categoryId, Category category); // admin auth
    bool Delete(int id); // admin auth
}

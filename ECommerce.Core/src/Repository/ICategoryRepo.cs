using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core;
public interface ICategoryRepo
{
    IEnumerable<Category> GetAll();
    Category CreateNew(Category category);// admin auth
    Category GetById(Guid id); // admin auth
    Category Update(Guid userId, Category category); // admin auth
    bool Delete(Guid id); // admin auth
}

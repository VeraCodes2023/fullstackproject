using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core;
public interface IProductRepo
{
    IEnumerable<Product> GetAll(ProductQueryParameters options);
    Product GetById(Guid id);
    Product Create(Product product); // 添加管理员权限验证逻辑
    Product Update(Guid id, Product product); // 添加管理员权限验证逻辑
    bool Delete(Guid id); // 添加管理员权限验证逻辑
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;

namespace ECommerce.Business;
public interface IProductService
{
    IEnumerable<ProductReadDTO> GetAllProducts(ProductQueryParameters options); // public 
    ProductReadDTO GetProductById(Guid id);// public 
    ProductReadDTO CreateProduct(ProductCreateDTO product);//Admin 
    ProdductUpdateReadDTO UpdateProduct(Guid id, ProductUpdateDTO product);//Admin 
    bool DeleteProduct(Guid id);//Admin 
}

  

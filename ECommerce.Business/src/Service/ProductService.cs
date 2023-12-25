using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core;

namespace ECommerce.Business;
public class ProductService : IProductService
{
    private readonly IProductRepo _productRepo;
    private IMapper _mapper;
    public ProductService(IProductRepo productRepo, IMapper mapper)
    {
        _productRepo = productRepo;
        _mapper = mapper;

    }

    public ProductReadDTO CreateProduct(ProductCreateDTO product)
    {
        if (product == null)
        {
           throw new Exception("bad request");
        }
        try
        {
            var newProduct = _mapper.Map<ProductCreateDTO, Product>(product);

            if(product.ImageCreateDTOs !=null && product.ImageCreateDTOs.Any())
            {
                foreach(var imageDTO in product.ImageCreateDTOs)
                {
                    _mapper.Map<ImageCreateDTO,Image>(imageDTO);
                }
            }
            var result= _productRepo.Create(newProduct);
            return _mapper.Map<Product, ProductReadDTO>(result);
        }
        catch(Exception)
        {
            throw;
        }
    }

    public bool DeleteProduct(Guid id)
    {
        if(id == Guid.Empty)
        {
             throw new Exception("bad request");
        }
        var targetProduct = _productRepo.GetById(id);
        if(targetProduct != null)
        {
            _productRepo.Delete(id);
            return true;
        }
        return false;
    }

    public IEnumerable<ProductReadDTO> GetAllProducts(ProductQueryParameters options)
    {
       try
        {
            var products = _productRepo.GetAll(options);
            if (products == null || !products.Any())
            {
                return Enumerable.Empty<ProductReadDTO>(); 
            }
           var productDTOs = products.Select(p => _mapper.Map<Product, ProductReadDTO>(p));
           if (!string.IsNullOrEmpty(options.Search))
            {
                productDTOs = productDTOs.Where(p => p.Title.Contains(options.Search)); // 标题包含搜索参数的产品
            }

           if (options.SortByPrice.HasValue)
            {
                productDTOs = options.SortByPrice switch
                {
                    SortDirection.Ascending => productDTOs.OrderBy(p => p.Price), 
                    SortDirection.Descending => productDTOs.OrderByDescending(p => p.Price),
                    _ => productDTOs 
                };
            }

            productDTOs = productDTOs.Skip(options.Offset).Take(options.Limit);

            return productDTOs;
        }
        catch(Exception)
        {
            throw new Exception("Failed to retrieve products");
        }

    }

    public ProductReadDTO GetProductById(Guid id)
    {
        if(id == Guid.Empty)
        {
            throw new Exception("bad request");
        }
        try
        {
            var targetProduct =_productRepo.GetById(id);
            if(targetProduct != null)
            {
                var mappedResult = _mapper.Map<ProductReadDTO>(targetProduct);
                mappedResult.ImageReadDTOs = _mapper.Map<List<ImageReadDTO>>(targetProduct.Images);
                return mappedResult;
            }
           throw new Exception("not found");
        }
        catch(Exception)
        {
            throw;
        }
    }

    public ProductReadDTO UpdateProduct(Guid id, ProductUpdateDTO product)
    {
        if (id == Guid.Empty || product == null)
        {
           throw new Exception("bad request");
        }
        
        try
        {
            var targetProduct =_productRepo.GetById(id);
            if (targetProduct == null)
            {
               throw new Exception("not found");
            }
            targetProduct!.Title = product.Title;
            targetProduct!.Price = product.Price;
            targetProduct.Description = product.Description;
            targetProduct.Inventory = product.Inventory;
            targetProduct.Images = _mapper.Map<List<Image>>(product.ImageUpdateDTOs);
            return _mapper.Map<ProductReadDTO>(targetProduct);
        }
        catch (Exception)
        {
            throw;
        }
    }
}

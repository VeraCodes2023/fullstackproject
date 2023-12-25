using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using ECommerce.Business;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWebAPI;
public class ProductRepo : IProductRepo
{
    protected DbSet<Product> _products;
    private DatabaseContext _database;

    public ProductRepo(DatabaseContext database)
    {
        _products = database.Products;
        _database = database;
    }
    public Product Create(Product product)
    {
        _products.Add(product);
        _database.SaveChanges();
        return product;
    }

    public bool Delete(Guid id)
    {
        var product = _products.Find(id);
        if(product !=null)
        {
            _products.Remove(product);
            _database.SaveChanges();
            return true;
        }
        return false;
    }

    public IEnumerable<Product> GetAll(ProductQueryParameters options)
    {
        return _products.AsNoTracking()
        .Include(p => p.Images)
        .Where(p=>p.Title.Contains(options.Search))
        .Skip(0)
        .Take(200);
    }
    public Product GetById(Guid id)
    {
        var foundProduct = _products.Include(p=>p.Images).FirstOrDefault(p=>p.Id ==id);
        return foundProduct!;
    }

    public Product Update(Guid id, Product product)
    {
        var existingProduct = _products.Find(id);
        existingProduct!.Title = product.Title;
        existingProduct.Price = product.Price;
        existingProduct.Description = product.Description;
        existingProduct.Images = product.Images;
        existingProduct.Inventory = product.Inventory;
        _products.Update(existingProduct);
        _database.SaveChanges();
        return existingProduct;
    }
}

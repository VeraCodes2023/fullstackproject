using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using ECommerce.Business;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWebAPI;
public class CategoryRepo : ICategoryRepo
{
    protected DbSet<Category> _categories;
    private DatabaseContext _database;

    public CategoryRepo(DatabaseContext database)
    {
        _database = database; 
        _categories =_database.Categories;
    }

    public Category CreateNew(Category category)
    {
        _categories.Add(category);
        _database.SaveChanges();
        return category;
    }

    public bool Delete(Guid id)
    {
        var category = _categories.Find(id);
        if(category !=null)
        {
            _categories.Remove(category);
            _database.SaveChanges();
            return true;
        }
        return false;
    }

    public IEnumerable<Category> GetAll()
    {
        return _categories.AsNoTracking()
        .Skip(0)
        .Take(50);
    }

    public Category GetById(Guid id)
    {
        var foundCategory = _categories.Find(id);
        return foundCategory!;
    }

    public Category Update(Guid id, Category category)
    {
        var existingCategory = _categories.Find(id);
        existingCategory!.Name = category.Name;
        existingCategory.Image =category.Image;
        _categories.Update(existingCategory);
        _database.SaveChanges();
        return existingCategory;
    }
}

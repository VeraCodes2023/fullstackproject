using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;

namespace ECommerce.Business;
public class ProductReadDTO
{
    public string Id { get; set; }
    public string Title { get; set; }
    public double Price { get; set; } 
    public int Inventory {get;set;}
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public List<ImageReadDTO> Images { get; set; }
    public DateTime CreatedAt   { get; set; }
    public ProductReadDTO()
    {
        CreatedAt = DateTime.UtcNow; 
    }
}

public class ProductCreateDTO
{
    public string Title { get; set; }
    public double Price { get; set; } 
    public int Inventory {get;set;}
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public List<ImageCreateDTO> Images { get; set; }
   
}


public class ProductUpdateDTO
{
    public string Title { get; set; }
    public double Price { get; set; } 
    public int Inventory {get;set;}
    public string Description { get; set; }
}

public class ProdductUpdateReadDTO
{
    public string Title { get; set; }
    public double Price { get; set; } 
    public int Inventory {get;set;}
    public string Description { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ProdductUpdateReadDTO()
    {
        UpdatedAt = DateTime.UtcNow; 
    }

}




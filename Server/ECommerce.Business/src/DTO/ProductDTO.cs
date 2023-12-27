using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;

namespace ECommerce.Business;
public class ProductReadDTO
{
    public string Title { get; set; }
    public double Price { get; set; } 
    public int Inventory {get;set;}
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public List<ImageReadDTO> ImageReadDTOs { get; set; }
}
public class ProductUpdateDTO
{
    public string Title { get; set; }
    public double Price { get; set; } 
    public int Inventory {get;set;}
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public List<ImageUpdateDTO> ImageUpdateDTOs { get; set; }
}

public class ProductCreateDTO
{
    public string Title { get; set; }
    public double Price { get; set; } 
    public int Inventory {get;set;}
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public List<ImageCreateDTO> ImageCreateDTOs { get; set; }
   
}


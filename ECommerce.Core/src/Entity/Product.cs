using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
namespace Core;
public class Product:BaseEntity
{    
    public string Title { get; set; }
    public double Price { get; set; } 
    public string Description { get; set; }
    public int Inventory {get;set;}
    
    [ForeignKey("CategoryId")]
    public Guid CategoryId { get; set; } //  fk navigate to category 
    public List<Image> Images { get; set; }
    
}

   
 
    


namespace Core;
using System.ComponentModel.DataAnnotations.Schema;

public class Image:BaseEntity
{
    [ForeignKey("ProductId")]
    public Guid ProductId { get; set; } // fk navigate to product 
    public string Url { get; set; }
    
}

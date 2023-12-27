using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core;
public class Review:BaseEntity
{
    [ForeignKey("UserId")]
    public Guid UserId { get; set; } // fk navigate to user 

    [ForeignKey("ProductId")]
    public Guid ProductId { get; set; } // fk navigate to product 
    public string Feedback { get; set; }
}

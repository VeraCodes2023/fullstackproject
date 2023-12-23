using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Core;

namespace Core;
public class BaseItem:BaseEntity
{   
    [ForeignKey("ProductId")]
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

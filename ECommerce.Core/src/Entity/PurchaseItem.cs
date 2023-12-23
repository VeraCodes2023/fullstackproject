using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core;
public class PurchaseItem:BaseItem
{
    [ForeignKey("PurchaseId")]
    public Guid PurchaseId {get;set;}  //fk navigate to order

    [ForeignKey("ProductId")]
    public Guid ProductId { get; set; } //fk navigate to product
    public int Quantity { get; set; }
}

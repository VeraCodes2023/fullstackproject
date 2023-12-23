using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;

namespace ECommerce.Business;
public class PurchaseItemReadDTO
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public double ProductPrice { get; set; }
    public int Quantity { get; set; }
}


public class PurchaseItemCreateDTO
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class PurchaseItemUpdateDTO
{
    public int Quantity { get; set; }
}


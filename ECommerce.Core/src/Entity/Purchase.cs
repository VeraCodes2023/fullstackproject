using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace Core;
public class Purchase:BaseEntity
{
    [ForeignKey("UserId")]
    public Guid UserId { get; set; } // fk navigate to user 
    public Status Status { get; set; } = Status.Pending;
    public List <PurchaseItem> PurchaseItems{ get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Completed,
    Cancelled
}


public class  OrderReadDTO:BaseEntity
{
    public Status Status { get; set; }  = Status.Pending;
    public List <PurchaseItem> PurchaseItems{ get; set; }
}
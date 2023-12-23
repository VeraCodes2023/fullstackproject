using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using ECommerce.Business;

namespace ECommerceBusiness;
public class  PurchaseReadDTO
{
    public Guid PurchaseId { get; set; }
    public Guid UserId { get; set; }
    public UserReadDTO User { get; set; } // User information
    public Status Status { get; set; }  = Status.Pending;
    public List <PurchaseItemReadDTO>  PurchaseItemReadDTOs{ get; set; }
}


public class  PurchaseCreateDTO
{
    public List <PurchaseItemCreateDTO> PurchaseItemCreateDTOs{ get; set; }
}


public class PurchaseUpdateDTO 
{
    public Status Status { get; set; }  
}
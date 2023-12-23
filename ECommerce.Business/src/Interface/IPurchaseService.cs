using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;

namespace ECommerceBusiness;
public interface IPurchaseService
{
    IEnumerable<PurchaseReadDTO> GetAllOrders(BaseQueryParameter options);
    PurchaseReadDTO GetOrderById(Guid id);
    PurchaseReadDTO CreateOrder(Guid userId, PurchaseCreateDTO  newOrder);
    bool CancelOrder(Guid id);
    PurchaseReadDTO UpdateOrderStatus(Guid id, PurchaseUpdateDTO updates);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;

namespace ECommerce.Business;
public interface IPurchaseService
{
    IEnumerable<PurchaseReadDTO> GetAllOrders(BaseQueryParameter options);
    PurchaseReadDTO GetOrderById(Guid id);
    PurchaseReadDTO CreateOrder(Guid userId, PurchaseCreateDTO  newOrder);
    bool CancelOrder(Guid id);
    PurchaseReadUpdateDTO UpdateOrderStatus(Guid id, PurchaseUpdateDTO updates);
}

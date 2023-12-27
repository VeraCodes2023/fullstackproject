using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core;
public interface IPurchaseRepo
{
    IEnumerable<Purchase>GetAll(BaseQueryParameter options); // logged in customer auth 
    Purchase CreateNew(Guid UserId, List<PurchaseItem> purchaseItems);// logged in customer auth 
    Purchase GetById(Guid id); // logged in customer auth 
    Purchase Update(Guid id, Purchase purchase); // admin auth
    bool Cancel(Guid id); // admin auth
}

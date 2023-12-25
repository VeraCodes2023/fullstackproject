using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using ECommerce.Business;
using ECommerceWebAPI;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWebAPI;
public class PurchaseRepo : IPurchaseRepo
{
    protected DbSet<Purchase> _orders;
    protected DbSet<PurchaseItem>_items;
    private DatabaseContext _database;

    public PurchaseRepo(DatabaseContext database)
    {
        _orders = database.Purchases;
        _items = database.PurchaseItems;
        _database = database;
    }

    public bool Cancel(Guid id)
    {
            var targetOrder = _orders.Find(id);
            _orders.Remove(targetOrder!);
            _database.SaveChanges();
            return true;
    }

    public Purchase CreateNew(Guid UserId, List<PurchaseItem> purchaseItems)
    {
        var newOrder = new Purchase
        {
            UserId = UserId,
            PurchaseItems = purchaseItems
        };
        _orders.Add(newOrder);
        _database.SaveChanges();
        return newOrder;
    }

    public IEnumerable<Purchase> GetAll(BaseQueryParameter options)
    {
            return _orders.AsNoTracking()
            .Include(p => p.PurchaseItems)
            .Skip(0)
            .Take(100)
            .ToList();
    }

    public Purchase GetById(Guid id)
    {
        var foundOrder = _orders.Find(id);
        return foundOrder!;
    }

    public Purchase Update(Guid id, Purchase purchase)
    {
        var foundOrder = _orders.Find(id);
        foundOrder!.Status= purchase.Status;
        _orders.Update(foundOrder);
        _database.SaveChanges();
        return foundOrder;
    }
}

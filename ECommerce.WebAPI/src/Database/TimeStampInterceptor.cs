using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Core;

namespace ECommerceWebAPI;
public class TimeStampInterceptor:SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var changedData = eventData.Context!.ChangeTracker.Entries(); // give collections of all entities experiencing the changes: Added or Updated, Deleted
            var updatedEntries = changedData.Where(entity => entity.State == EntityState.Modified);
            var addedEntries = changedData.Where(entity => entity.State == EntityState.Added);

            foreach(var e in updatedEntries)
            {
                if(e.Entity is BaseEntity entity)
                {
                    entity.UpdatedAt = DateTime.Now;
                }
            }

            foreach (var e in addedEntries)
            {
                if (e.Entity is BaseEntity entity)
                {
                    entity.UpdatedAt = DateTime.Now;
                    entity.CreatedAt = DateTime.Now;
                }
            }
            return base.SavingChanges(eventData, result);
        }
}

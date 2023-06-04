using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchitecture.Infrastructure.Persistence.Interceptors;

public class DeletableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<IDeletableEntity>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.Entity.IsDeleted = true;
                entry.State = EntityState.Modified;

                var owneds = entry.GetChangedOwnedEntities();

                foreach (var owned in owneds)
                {
                    owned.State = EntityState.Modified;
                }
            }
        }
    }
}

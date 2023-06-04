using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanArchitecture.Infrastructure.Persistence;

public static class EntityEntryExtensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));

    public static IEnumerable<EntityEntry> GetChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Select(r => r.TargetEntry!)
            .Where(e => e is not null)
            .Where(e => e.Metadata.IsOwned());
}

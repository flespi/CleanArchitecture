using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace CleanArchitecture.Infrastructure.Data.ValueGeneration;

public class ChronologicalGuidValueGenerator : ValueGenerator<Guid>
{
    public override Guid Next(EntityEntry entry)
        => Guid.CreateVersion7();

    public override bool GeneratesTemporaryValues
        => false;
}

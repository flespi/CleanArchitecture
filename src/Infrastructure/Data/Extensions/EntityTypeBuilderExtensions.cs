using CleanArchitecture.Domain.Common;
using CleanArchitecture.Infrastructure.Data.ValueConversion;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<TEntity> HasConcurrencyToken<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IConcurrentEntity
    {
        builder.Property(x => x.ConcurrencyToken)
            .HasConversion<HexToBytesConverter>()
            .IsRowVersion();

        return builder;
    }
}

using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<TEntity> HasConcurrencyToken<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IConcurrentEntity
    {
        builder.Property(x => x.ConcurrencyToken).IsRowVersion();
        return builder;
    }
}

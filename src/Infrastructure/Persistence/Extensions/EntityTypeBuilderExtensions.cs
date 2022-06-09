using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<TEntity> HasIdempotencyKey<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IIdempotentEntity
    {
        builder.HasIndex(x => x.IdempotencyKey).IsUnique();
        return builder;
    }
}

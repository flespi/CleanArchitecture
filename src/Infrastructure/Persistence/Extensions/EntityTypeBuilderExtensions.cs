using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<TEntity> HasUniqueId<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseEntity
    {
        builder.HasKey(e => e.Id)
            .IsClustered(false);

        builder.HasAlternateKey(e => e.Sequence)
            .IsClustered(true);

        builder.Property(e => e.Id)
            .HasDefaultValueSql("(newid())");

        builder.Property(e => e.Sequence)
            .ValueGeneratedOnAdd();

        return builder;
    }

    public static EntityTypeBuilder<TEntity> HasConcurrencyToken<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IConcurrentEntity
    {
        builder.Property(x => x.ConcurrencyToken).IsRowVersion();
        return builder;
    }

    public static EntityTypeBuilder<TEntity> HasDeletedFlag<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IDeletableEntity
    {
        builder.Property(x => x.IsDeleted);
        builder.HasIndex(x => x.IsDeleted);

        builder.HasQueryFilter(x => !x.IsDeleted);

        return builder;
    }
}

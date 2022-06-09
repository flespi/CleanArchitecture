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

        builder.HasIndex(e => e.Sequence)
            .IsUnique()
            .IsClustered();

        builder.Property(e => e.Id)
            .HasDefaultValueSql("(newid())");

        builder.Property(e => e.Sequence)
            .ValueGeneratedOnAdd();

        return builder;
    }
}

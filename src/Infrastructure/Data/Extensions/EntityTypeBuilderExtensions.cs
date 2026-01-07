using CleanArchitecture.Domain.Common;
using CleanArchitecture.Infrastructure.Data.ValueConversion;
using CleanArchitecture.Infrastructure.Data.ValueGeneration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<TEntity> HasUniqueId<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseEntity
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion<GuidToBytesConverter>()
            .HasValueGenerator<ChronologicalGuidValueGenerator>()
            .ValueGeneratedOnAdd();

        return builder;
    }
}

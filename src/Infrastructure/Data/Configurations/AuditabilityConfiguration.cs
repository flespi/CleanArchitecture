using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class AuditabilityConfiguration<TOwnerEntity> : IOwnedNavigationConfiguration<TOwnerEntity, Auditability>
    where TOwnerEntity : class
{
    public void Configure(OwnedNavigationBuilder<TOwnerEntity, Auditability> builder)
    {
        builder.Property(e => e.Created)
            .IsRequired();

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.LastModified)
            .IsRequired();

        builder.Property(e => e.LastModifiedBy)
            .HasMaxLength(200)
            .IsRequired();
    }
}

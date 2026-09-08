using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public interface IOwnedNavigationConfiguration<TOwnerEntity, TDependentEntity>
    where TOwnerEntity : class
    where TDependentEntity : class
{
    public void Configure(OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder);
}

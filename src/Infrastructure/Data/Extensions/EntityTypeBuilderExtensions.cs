using System.Linq.Expressions;
using CleanArchitecture.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<TEntity> OwnsOne<TEntity, TRelatedEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, TRelatedEntity?>> navigationExpression,
        IOwnedNavigationConfiguration<TEntity, TRelatedEntity> navigationConfiguration)
        where TEntity : class
        where TRelatedEntity : class
    {
        return builder.OwnsOne(navigationExpression, b => navigationConfiguration.Configure(b));
    }
}

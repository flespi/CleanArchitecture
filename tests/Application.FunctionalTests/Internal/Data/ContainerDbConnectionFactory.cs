using System.Data.Common;
using CleanArchitecture.Infrastructure.Data;
using DotNet.Testcontainers.Containers;

namespace CleanArchitecture.Application.FunctionalTests.Internal.Data;

public class ContainerDbConnectionFactory : IDbConnectionFactory
{
    private readonly IDatabaseContainer _container;
    private readonly DbProviderFactory _providerFactory;

    public ContainerDbConnectionFactory(
        IDatabaseContainer container,
        DbProviderFactory providerFactory)
    {
        _container = container;
        _providerFactory = providerFactory;
    }

    public DbConnection CreateConnection()
    {
        var connectionString = _container.GetConnectionString();
        return _providerFactory.CreateConnection(connectionString)!;
    }
}

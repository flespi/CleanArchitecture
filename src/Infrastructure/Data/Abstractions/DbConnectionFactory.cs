using System.Data.Common;

namespace CleanArchitecture.Infrastructure.Data;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly Func<DbConnection> _factory;

    public DbConnectionFactory(Func<DbConnection> factory)
    {
        _factory = factory;
    }

    public DbConnection CreateConnection() => _factory();
}

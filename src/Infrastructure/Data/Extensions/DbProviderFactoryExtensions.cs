using System.Data.Common;

namespace CleanArchitecture.Infrastructure.Data;

public static class DbProviderFactoryExtensions
{
    public static DbConnection? CreateConnection(this DbProviderFactory factory, string connectionString)
    {
        var connection = factory.CreateConnection();
        connection?.ConnectionString = connectionString;
        return connection;
    }
}

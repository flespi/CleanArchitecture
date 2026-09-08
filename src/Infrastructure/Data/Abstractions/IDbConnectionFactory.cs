using System.Data.Common;

namespace CleanArchitecture.Infrastructure.Data;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection();
}

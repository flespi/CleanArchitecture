using CleanArchitecture.Infrastructure.Data.Transactions;

namespace CleanArchitecture.Infrastructure.Data;

public class ApplicationDb : EntityFrameworkDb<ApplicationDbContext>
{
    public ApplicationDb(ApplicationDbContext context) : base(context)
    {
    }
}

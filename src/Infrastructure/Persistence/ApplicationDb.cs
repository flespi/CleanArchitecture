using CleanArchitecture.Infrastructure.Persistence.Transactions;

namespace CleanArchitecture.Infrastructure.Persistence;

public class ApplicationDb : EntityFrameworkDb<ApplicationDbContext>
{
    public ApplicationDb(ApplicationDbContext context) : base(context)
    {
    }
}

using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Account.Data.IdentityProvider;

public class IdentityProviderDbContext : DbContext
{
    public IdentityProviderDbContext(DbContextOptions<IdentityProviderDbContext> options)
        : base(options)
    {
    }
}

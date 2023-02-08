using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Account.Data.Identity;

public class IdentityDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }
}

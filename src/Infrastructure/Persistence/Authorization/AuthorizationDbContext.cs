using Balea.EntityFrameworkCore.Store.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence.Authorization;

public class AuthorizationDbContext : BaleaDbContext
{
    public AuthorizationDbContext(
        DbContextOptions<AuthorizationDbContext> options)
        : base(options)
    {
    }
}

using Balea;
using Balea.EntityFrameworkCore.Store.Entities;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Persistence.Authorization;

public class AuthorizationDbContextInitialiser : IDataInitialiser
{
    private readonly ILogger<AuthorizationDbContextInitialiser> _logger;
    private readonly AuthorizationDbContext _context;

    public AuthorizationDbContextInitialiser(ILogger<AuthorizationDbContextInitialiser> logger, AuthorizationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default data
        // Seed, if necessary
        if (!_context.Roles.Any())
        {
            var application = new ApplicationEntity(BaleaConstants.DefaultApplicationName, "Default application");

            var canPurgePermission = new PermissionEntity(Permissions.CanPurge);
            application.Permissions.Add(canPurgePermission);

            var adminMapping = new MappingEntity("Administrator");

            var adminRole = new RoleEntity(Roles.Administrator, "Administrator");
            adminRole.Permissions.Add(new RolePermissionEntity { Permission = canPurgePermission });
            adminRole.Mappings.Add(new RoleMappingEntity { Mapping = adminMapping });
            application.Roles.Add(adminRole);

            _context.Applications.Add(application);

            await _context.SaveChangesAsync();
        }
    }
}

using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orca;

namespace CleanArchitecture.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static void AddAsyncSeeding(this DbContextOptionsBuilder builder, IServiceProvider serviceProvider)
    {
        builder.UseAsyncSeeding(async (context, _, ct) =>
        {
            var initialiser = serviceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

            await initialiser.SeedAsync();
        });
    }

    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly ISubjectStore _subjectStore;
    private readonly IRoleStore _roleStore;
    private readonly IPermissionStore _permissionStore;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, ISubjectStore subjectStore, IRoleStore roleStore, IPermissionStore permissionStore)
    {
        _logger = logger;
        _context = context;
        _subjectStore = subjectStore;
        _roleStore = roleStore;
        _permissionStore = permissionStore;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
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
        var canPurgePermission = new Permission
        {
            Name = Policies.CanPurge
        };

        await _permissionStore.CreateAsync(canPurgePermission);

        // Default roles
        var administratorRole = await _roleStore.FindByNameAsync(Roles.Administrator);

        if (administratorRole is null)
        {
            administratorRole = new Role
            {
                Name = Roles.Administrator,
            };

            await _roleStore.CreateAsync(administratorRole);
            await _roleStore.AddPermissionAsync(administratorRole, canPurgePermission);
        }

        // Default users
        var administratorResult = await _subjectStore.SearchAsync(new SubjectFilter { Name = "administrator@localhost" });
        var administrator = administratorResult.FirstOrDefault();

        if (administrator is null)
        {
            administrator = new Subject
            {
                Sub = Guid.NewGuid().ToString(),
                Name = "administrator@localhost",
                Email = "administrator@localhost"
            };

            await _subjectStore.CreateAsync(administrator);
            await _subjectStore.AddRoleAsync(administrator, administratorRole);
        }

        // Default data
        // Seed, if necessary
        if (!_context.TodoLists.Any())
        {
            _context.TodoLists.Add(new TodoList
            {
                Title = "Todo List",
                Items =
                {
                    new TodoItem { Title = "Make a todo list 📃" },
                    new TodoItem { Title = "Check off the first item ✅" },
                    new TodoItem { Title = "Realise you've already done two things on the list! 🤯"},
                    new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
                }
            });

            await _context.SaveChangesAsync();
        }
    }
}

using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Identity;
using EFSeeder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Data.Seeders;

[DbContext(typeof(ApplicationDbContext))]
[DataSeeder("00000000000000_InitialSeed")]
public class InitialSeed : IDataSeeder<ApplicationDbContext>
{
    private readonly ILogger<InitialSeed> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public InitialSeed(ILogger<InitialSeed> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
    {
        // Default roles
        var administratorRole = new IdentityRole(Roles.Administrator);

        await _roleManager.CreateAsync(administratorRole);

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        await _userManager.CreateAsync(administrator, "Administrator1!");
        if (!string.IsNullOrWhiteSpace(administratorRole.Name))
        {
            await _userManager.AddToRolesAsync(administrator, [administratorRole.Name]);
        }

        // Default data
        // Seed, if necessary
        context.TodoLists.Add(new TodoList
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

        await context.SaveChangesAsync();
    }
}

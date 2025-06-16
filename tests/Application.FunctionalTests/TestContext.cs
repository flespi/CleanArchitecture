using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Identity;
using DotNet.Testcontainers.Containers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
#if (UsePostgreSQL)
using Testcontainers.PostgreSql;
#else
using Testcontainers.MsSql;
#endif

namespace CleanArchitecture.Application.FunctionalTests;

public class TestContext : IAsyncLifetime
{
    private readonly CurrentUser _user;

    private readonly IDatabaseContainer _container;
    private readonly WebApplicationFactory<Program> _factory;

    private readonly Lazy<IServiceScopeFactory> _scopeFactory;

    private Respawner _checkpoint = null!;

    public TestContext()
    {
        _user = new CurrentUser();

#if (UsePostgreSQL)
        _container = new PostgreSqlBuilder().Build();
#else
        _container = new MsSqlBuilder().Build();
#endif

        _factory = new CustomWebApplicationFactory(_container, _user);

        // This needs to be evaluated after InitializeAsync
        _scopeFactory = new(CreateScopeFactory);
    }

    private IServiceScopeFactory CreateScopeFactory()
    {
        return _factory.Services.GetRequiredService<IServiceScopeFactory>();
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.Value.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public async Task SendAsync(IBaseRequest request)
    {
        using var scope = _scopeFactory.Value.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        await mediator.Send(request);
    }

    public string? GetCurrentUserId()
    {
        return _user.Id;
    }

    public async Task<string> RunAsDefaultUserAsync()
    {
        return await RunAsUserAsync("test@local", "Testing1234!", Array.Empty<string>());
    }

    public async Task<string> RunAsAdministratorAsync()
    {
        return await RunAsUserAsync("administrator@local", "Administrator1234!", new[] { "Administrator" });
    }

    public async Task<string> RunAsUserAsync(string userName, string password, string[] roles)
    {
        using var scope = _scopeFactory.Value.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var user = new ApplicationUser { UserName = userName, Email = userName };

        var result = await userManager.CreateAsync(user, password);

        if (roles.Any())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            await userManager.AddToRolesAsync(user, roles);
        }

        if (result.Succeeded)
        {
            _user.Id = user.Id;

            return _user.Id;
        }

        var errors = string.Join(Environment.NewLine, result.ToApplicationResult().Errors);

        throw new Exception($"Unable to create {userName}.{Environment.NewLine}{errors}");
    }

    public async Task ResetState()
    {
        try
        {
            await _checkpoint.ResetAsync(_container.GetConnectionString());
        }
        catch (Exception) 
        {
        }

        _user.Id = null;
    }

    public async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.Value.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.Value.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = _scopeFactory.Value.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    private async Task<Respawner> CreateCheckpoint()
    {
        using var scope = _scopeFactory.Value.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync(); ;

        return await Respawner.CreateAsync(_container.GetConnectionString(), new RespawnerOptions
        {
#if (UsePostgreSQL)
            DbAdapter = DbAdapter.Postgres,
#endif
            TablesToIgnore = ["__EFMigrationsHistory"]
        });
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        _checkpoint = await CreateCheckpoint();
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
    }
}

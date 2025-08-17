using CleanArchitecture.Infrastructure.Data;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Mvc.Testing;
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
    public CurrentUser User { get; }

    private readonly IDatabaseContainer _container;
    private readonly WebApplicationFactory<Program> _factory;

    private readonly Lazy<IServiceScopeFactory> _scopeFactory;

    private Respawner _checkpoint = null!;

    public TestContext()
    {
        User = new CurrentUser();

#if (UsePostgreSQL)
        _container = new PostgreSqlBuilder().Build();
#else
        _container = new MsSqlBuilder().Build();
#endif

        _factory = new CustomWebApplicationFactory(_container, User);

        // This needs to be evaluated after InitializeAsync
        _scopeFactory = new(CreateScopeFactory);
    }

    public IServiceScope CreateScope()
    {
        return _scopeFactory.Value.CreateScope();
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

    public async Task ResetState()
    {
        try
        {
            await _checkpoint.ResetAsync(_container.GetConnectionString());
        }
        catch (Exception) 
        {
        }

        User.Id = null;
    }

    private IServiceScopeFactory CreateScopeFactory()
    {
        return _factory.Services.GetRequiredService<IServiceScopeFactory>();
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
}

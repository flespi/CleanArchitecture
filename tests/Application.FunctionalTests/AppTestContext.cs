using CleanArchitecture.Application.FunctionalTests.Internal.Abstractions;
using CleanArchitecture.Application.FunctionalTests.Internal.Snapshot;
using CleanArchitecture.Infrastructure.Data;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

#if (UsePostgreSQL)
using Testcontainers.PostgreSql;
#else
using Testcontainers.MsSql;
#endif

namespace CleanArchitecture.Application.FunctionalTests;

public class AppTestContext : IAppTestContext
{
    private readonly IDatabaseContainer _container;
    private readonly IAsyncEcoLifetime _snapshot;

    private readonly WebApplicationFactory _factory;

    public IServiceScopeFactory ServiceScopeFactory => _factory.Services.GetRequiredService<IServiceScopeFactory>();

    public CurrentUser User { get; }

    public AppTestContext()
    {
        User = new CurrentUser();

        _snapshot = CreateTestDatabase(out _container);

        _factory = WebApplicationFactory.Create(builder =>
        {
            var connectionString = _container.GetConnectionString();

            builder
                .UseEnvironment("Testing")
                .UseSetting("ConnectionStrings:CleanArchitectureDb", connectionString);

            builder.ConfigureTestServices(services =>
            {
                services.AddTestServices(new()
                {
                    Container = _container,
                    User = User,
                });
            });
        });
    }

    private IAsyncEcoLifetime CreateTestDatabase(out IDatabaseContainer container)
    {
#if (UsePostgreSQL)
        const string image = "postgres";
        const string tag = "18.6";

        container = new PostgreSqlBuilder($"{image}:{tag}")
            .WithDatabase("CleanArchitecture")
            .Build();
#else
        const string image = "mcr.microsoft.com/mssql/server";
        const string tag = "2022-CU26-ubuntu-22.04";

        container = new MsSqlBuilder($"{image}:{tag}")
            .WithDatabase("CleanArchitecture")
            .Build();
#endif

        return new DatabaseSnapshotBuilder()
            .WithOptions(new()
            {
                TablesToIgnore = ["__EFMigrationsHistory"]
            })
            .Build(container, SqlClientFactory.Instance);
    }

    public async ValueTask InitializeAsync()
    {
        await _container.StartAsync();

        await SeedDataAsync();
        await _snapshot.InitializeAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _snapshot.DisposeAsync();
        await _container.StopAsync();
    }

    public async ValueTask ResetAsync()
    {
        User.Id = null;
        User.Roles = null;

        await _snapshot.ResetAsync();
        await SeedDataAsync();
    }

    private async Task SeedDataAsync()
    {
        using var scope = ServiceScopeFactory.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Data;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CleanArchitecture.Application.FunctionalTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly IDatabaseContainer _container;

    private readonly IUser _user;

    public CustomWebApplicationFactory(IDatabaseContainer container, IUser user)
    {
        _container = container;
        _user = user;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connectionString = _container.GetConnectionString();

        builder
            .UseEnvironment("Testing")
            .UseSetting("ConnectionStrings:CleanArchitectureDb", connectionString);

        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<IUser>()
                .AddSingleton(_user);
#if (!UseAspire || UseSqlite)
            services
                .RemoveAll<DbContextOptions<ApplicationDbContext>>()
                .AddDbContext<ApplicationDbContext>((sp, options) =>
                {
                    options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
#if (UsePostgreSQL)
                    options.UseNpgsql(connectionString);
#elif (UseSqlite)
                    options.UseSqlite(connectionString);
#else
                    options.UseSqlServer(connectionString);
    #endif
                });
#endif
        });
    }
}

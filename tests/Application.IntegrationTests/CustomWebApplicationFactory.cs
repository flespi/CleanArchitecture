using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application.IntegrationTests;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly ICurrentUserService _currentUserService;

    public CustomWebApplicationFactory(CurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            var integrationConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            configurationBuilder.AddConfiguration(integrationConfig);
        });

        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(_currentUserService);

            services
                .Remove<DbContextOptions<ApplicationDbContext>>()
                .AddDbContext<ApplicationDbContext>((sp, options) =>
                    options.UseSqlServer(sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection"),
                        builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        });
    }
}

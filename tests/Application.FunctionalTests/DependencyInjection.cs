using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CleanArchitecture.Application.FunctionalTests;

public static class DependencyInjection
{
    public static void AddTestServices(this IServiceCollection services, RegistralOptions registralOptions)
    {
        services
                .RemoveAll<IUser>()
                .AddSingleton(registralOptions.User);
#if (!UseAspire || UseSqlite)
        services
            .RemoveAll<DbContextOptions<ApplicationDbContext>>()
            .AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var connectionString = registralOptions.Container.GetConnectionString();

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
    }
}

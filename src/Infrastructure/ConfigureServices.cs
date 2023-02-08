using System.IdentityModel.Tokens.Jwt;
using Balea;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Files;
using CleanArchitecture.Infrastructure.Persistence.Application;
using CleanArchitecture.Infrastructure.Persistence.Authorization;
using CleanArchitecture.Infrastructure.Persistence.Interceptors;
using CleanArchitecture.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("CleanArchitecture"));

            services.AddDbContext<AuthorizationDbContext>(options =>
                options.UseInMemoryDatabase("Authorization"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddDbContext<AuthorizationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Authorization"),
                    builder => builder.MigrationsAssembly(typeof(AuthorizationDbContext).Assembly.FullName)));
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IDataInitialiser, ApplicationDbContextInitialiser>();
        services.AddScoped<IDataInitialiser, AuthorizationDbContextInitialiser>();

        services.AddTransient<IPrincipalService, PrincipalService>();

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                configuration.GetSection("Authentication").Bind(options);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = JwtClaimTypes.Role,
                    NameClaimType = JwtClaimTypes.Name
                };
            });

        services.AddBalea(options =>
            {
                options.DefaultClaimTypeMap = new DefaultClaimTypeMap
                {
                    RoleClaimType = JwtClaimTypes.Role,
                    NameClaimType = JwtClaimTypes.Name,
                };

                options.DefaultClaimTypeMap.AllowedSubjectClaimTypes.Clear();
                options.DefaultClaimTypeMap.AllowedSubjectClaimTypes.Add(JwtClaimTypes.Subject);
            })
            .AddEntityFrameworkCoreStore<AuthorizationDbContext>();

        return services;
    }
}

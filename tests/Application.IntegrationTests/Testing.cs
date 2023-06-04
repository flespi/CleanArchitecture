using System.Security.Claims;
using Balea;
using CleanArchitecture.Infrastructure.Persistence.Application;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Respawn;

namespace CleanArchitecture.Application.IntegrationTests;

[SetUpFixture]
public partial class Testing
{
    private static WebApplicationFactory<Program> _factory = null!;
    private static IConfiguration _configuration = null!;
    private static IServiceScopeFactory _scopeFactory = null!;
    private static Respawner _checkpoint = null!;
    private static ClaimsPrincipal? _currentUser;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        _factory = new CustomWebApplicationFactory();
        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
        _configuration = _factory.Services.GetRequiredService<IConfiguration>();

        _checkpoint = Respawner.CreateAsync(_configuration.GetConnectionString("DefaultConnection")!, new RespawnerOptions
        {
            TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" }
        }).GetAwaiter().GetResult();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public static async Task SendAsync(IBaseRequest request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        await mediator.Send(request);
    }

    public static ClaimsPrincipal? GetCurrentUser()
    {
        return _currentUser;
    }

    public static async Task<string> RunAsDefaultUserAsync()
    {
        return await RunAsUserAsync("test@local", "Testing1234!", Array.Empty<string>());
    }

    public static async Task<string> RunAsAdministratorAsync()
    {
        return await RunAsUserAsync("administrator@local", "Administrator1234!", new[] { "Administrator" });
    }

    public static async Task<string> RunAsUserAsync(string userName, string password, string[] roles)
    {
        var claims = new List<Claim>
        {
            new(JwtClaimTypes.Subject, userName)
        };

        foreach (var role in roles)
        {
            claims.Add(new(JwtClaimTypes.Role, role));
        }

        var identity = new ClaimsIdentity(claims, "Password", JwtClaimTypes.Name, JwtClaimTypes.Role);

        var principal = new ClaimsPrincipal();
        principal.AddIdentity(identity);

        await AuthenticateAsync(principal);

        _currentUser = principal;

        return userName;
    }

    private static async Task AuthenticateAsync(ClaimsPrincipal principal)
    {
        using var scope = _scopeFactory.CreateScope();

        var policyProvider = scope.ServiceProvider.GetRequiredService<IAuthorizationPolicyProvider>();
        var policyEvaluator = scope.ServiceProvider.GetRequiredService<IPolicyEvaluator>();

        var defaultPolicy = await policyProvider.GetDefaultPolicyAsync();
        var httpContext = new DefaultHttpContext { User = principal };

        var result = await policyEvaluator.AuthenticateAsync(defaultPolicy, httpContext);
    }

    public static async Task ResetState()
    {
        try
        {
            await _checkpoint.ResetAsync(_configuration.GetConnectionString("DefaultConnection")!);
        }
        catch (Exception) 
        {
        }

        _currentUser = null;
    }

    public static async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public static async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}

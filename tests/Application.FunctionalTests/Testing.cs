using System.Security.Claims;
using System.Security.Principal;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Orca;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Orca;

namespace CleanArchitecture.Application.FunctionalTests;

[SetUpFixture]
public partial class Testing
{
    private static ITestDatabase _database = null!;
    private static CustomWebApplicationFactory _factory = null!;
    private static IServiceScopeFactory _scopeFactory = null!;
    private static CurrentUser _user = null!;

    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        _user = new CurrentUser();

        _database = await TestDatabaseFactory.CreateAsync();

        _factory = new CustomWebApplicationFactory(_database.GetConnection(), _database.GetConnectionString(), _user);

        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
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

    public static ClaimsPrincipal? GetUserId()
    {
        return _user.Principal;
    }

    public static async Task<string> RunAsDefaultUserAsync()
    {
        return await RunAsUserAsync("test@local", "Testing1234!", Array.Empty<string>());
    }

    public static async Task<string> RunAsAdministratorAsync()
    {
        return await RunAsUserAsync("administrator@local", "Administrator1234!", new[] { Roles.Administrator });
    }

    public static async Task<string> RunAsUserAsync(string userName, string password, string[] roles)
    {
        using var scope = _scopeFactory.CreateScope();

        var subjectStore = scope.ServiceProvider.GetRequiredService<ISubjectStore>();

        var subject = new Subject { Sub = Guid.NewGuid().ToString(), Name = userName, Email = userName };

        var result = await subjectStore.CreateAsync(subject);

        if (roles.Any())
        {
            var roleStore = scope.ServiceProvider.GetRequiredService<IRoleStore>();

            foreach (var role in roles)
            {
                var subjectRole = new Role { Name = role };

                await roleStore.CreateAsync(subjectRole);
                await subjectStore.AddRoleAsync(subject, subjectRole);
            }
        }

        if (result.Succeeded)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, subject.Sub),
                new(ClaimTypes.Name, userName),
            };

            foreach (var role in roles)
            {
                claims.Add(new(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, "Password", ClaimTypes.Name, ClaimTypes.Role);

            var principal = new ClaimsPrincipal();
            principal.AddIdentity(identity);

            var authorizationContextProvider = scope.ServiceProvider.GetRequiredService<IAuthorizationContextProvider>();
            var authorizationContext = await authorizationContextProvider.CreateAsync(principal);

            var orcaIdentity = new ClaimsIdentityFactory(new()).Create(authorizationContext);
            principal.AddIdentity(orcaIdentity);

            _user.Principal = principal;

            return subject.Sub;
        }

        var errors = string.Join(Environment.NewLine, result.ToApplicationResult().Errors);

        throw new Exception($"Unable to create {userName}.{Environment.NewLine}{errors}");
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
            await _database.ResetAsync();
        }
        catch (Exception) 
        {
        }

        _user.Principal = null;
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
    public async Task RunAfterAnyTests()
    {
        await _database.DisposeAsync();
        await _factory.DisposeAsync();
    }
}

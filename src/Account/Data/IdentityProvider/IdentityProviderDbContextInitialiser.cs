using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

namespace CleanArchitecture.Account.Data.IdentityProvider;

public class IdentityProviderDbContextInitialiser : IDataInitialiser
{
    private readonly ILogger<IdentityProviderDbContextInitialiser> _logger;
    private readonly IConfiguration _configuration;

    private readonly IOpenIddictApplicationManager _clientManager;
    private readonly IOpenIddictScopeManager _scopeManager;

    private readonly IdentityProviderDbContext _context;

    public IdentityProviderDbContextInitialiser(
        ILogger<IdentityProviderDbContextInitialiser> logger,
        IConfiguration configuration,
        IOpenIddictApplicationManager clientManager,
        IOpenIddictScopeManager scopeManager,
        IdentityProviderDbContext context
        )
    {
        _logger = logger;
        _configuration = configuration;
        _clientManager = clientManager;
        _scopeManager = scopeManager;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        var scopes = _configuration.GetSection("Oidc:Scopes").Get<OpenIddictScopeDescriptor[]>();
        var clients = _configuration.GetSection("Oidc:Clients").Get<OpenIddictApplicationDescriptor[]>();

        if (scopes is not null)
        {
            if (!await _scopeManager.ListAsync().AnyAsync())
            {
                foreach (var scope in scopes)
                {
                    await _scopeManager.CreateAsync(scope);
                }
            }
        }

        if (clients is not null)
        {
            if (!await _clientManager.ListAsync().AnyAsync())
            {
                foreach (var client in clients)
                {
                    await _clientManager.CreateAsync(client);
                }
            }
        }
    }
}
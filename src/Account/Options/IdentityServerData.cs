using Duende.IdentityServer.Models;

namespace CleanArchitecture.Account;

public class IdentityServerData
{
    public IEnumerable<IdentityResource> IdentityResources { get; set; }

    public IEnumerable<ApiScope> ApiScopes { get; set; }

    public IEnumerable<Client> Clients { get; set; }
}
using System.Security.Claims;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IPrincipalService
{
    Task CreateOrUpdateAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);
}

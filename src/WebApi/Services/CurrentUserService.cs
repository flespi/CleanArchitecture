using System.Security.Claims;

using CleanArchitecture.Application.Common.Interfaces;
using IdentityModel;

namespace CleanArchitecture.WebApi.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtClaimTypes.Subject);
}

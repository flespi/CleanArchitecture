using System.Security.Claims;
using CleanArchitecture.Application.Common.Identity;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Web.Services;

public class IdentityResolver : IIdentityResolver
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityResolver(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public IUser User => new BasicUser(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!, []);
}

using System.Security.Claims;

namespace CleanArchitecture.Application.Common;

public static class ClaimsPrincipalExtensions
{
    public static string? GetIdentifier(this ClaimsPrincipal principal) => principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}

using Balea;

namespace System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static string? GetSubjectId(this ClaimsPrincipal principal)
        => principal.FindFirstValue(JwtClaimTypes.Subject);
}

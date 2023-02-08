using System.Security.Claims;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface ICurrentUserService
{
    ClaimsPrincipal? User { get; }
}

using System.Security.Claims;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IUser
{
    ClaimsPrincipal? Principal { get; }
}

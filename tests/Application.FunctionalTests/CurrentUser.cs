using System.Security.Claims;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.FunctionalTests;

public class CurrentUser : IUser
{
    public ClaimsPrincipal? Principal { get; set; }
}

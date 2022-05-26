namespace CleanArchitecture.Application.IntegrationTests;

using CleanArchitecture.Application.Common.Interfaces;

public class CurrentUserService : ICurrentUserService
{
    public string? UserId { get; set; }
}

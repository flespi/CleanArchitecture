using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.FunctionalTests;

public class CurrentUser : IUser
{
    public string? Id { get; set; }

    public List<string>? Roles { get; set; }
}

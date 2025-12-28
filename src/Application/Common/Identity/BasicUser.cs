using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Common.Identity;

public class BasicUser(string id, List<string> roles) : IUser
{
    public string Id { get; } = id;

    public List<string>? Roles { get; } = roles;
}

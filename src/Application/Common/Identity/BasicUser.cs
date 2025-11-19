using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Common.Identity;

public class BasicUser(string id) : IUser
{
    public string Id { get; } = id;
}

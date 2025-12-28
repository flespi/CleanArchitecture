using CleanArchitecture.Application.Common.Identity;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.FunctionalTests;

public class IdentityResolver(string id, List<string> roles) : IIdentityResolver
{
    public IUser User { get; } = new BasicUser(id, roles);
}

using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Common.Identity;

public class IdentityAccessor(IIdentityResolver resolver) : IIdentityAccessor
{
    private readonly IdentityContext _context = new(resolver.User);

    public IUser User => _context.User;

    public IImpersonation Impersonate(IUser user)
        => new Impersonation(_context, user);
}

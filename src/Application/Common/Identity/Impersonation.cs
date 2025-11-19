using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Common.Identity;

public class Impersonation : IImpersonation
{
    private bool _disposed = false;

    private readonly IdentityContext _context;

    private readonly IUser _previousUser;
    private readonly IUser _newUser;

    public IUser Identity => _newUser;

    public Impersonation(IdentityContext context, IUser user)
    {
        _context = context;
        _newUser = user;

        _context.Swap(_newUser, out _previousUser);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _context.Swap(_previousUser, out IUser _);
            _disposed = true;
        }
    }
}

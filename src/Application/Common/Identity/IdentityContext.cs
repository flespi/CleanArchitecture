using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Common.Identity;

public class IdentityContext(IUser user)
{
    public IUser User { get; private set; } = user;

    public void Swap(IUser current, out IUser previous)
    {
        previous = User;
        User = current;
    }
}

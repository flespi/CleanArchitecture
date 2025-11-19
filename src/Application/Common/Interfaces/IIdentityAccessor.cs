namespace CleanArchitecture.Application.Common.Interfaces;

public interface IIdentityAccessor
{
    IUser User { get; }

    IImpersonation Impersonate(IUser user);
}

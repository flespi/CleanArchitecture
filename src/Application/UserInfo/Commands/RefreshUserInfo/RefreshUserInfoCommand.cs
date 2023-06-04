using CleanArchitecture.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.UserInfo.Commands.RefreshUserInfo;

public record RefreshUserInfoCommand : IRequest
{
}

public class RefreshUserInfoCommandHandler : IRequestHandler<RefreshUserInfoCommand>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IPrincipalService _authorization;

    public RefreshUserInfoCommandHandler(
        ICurrentUserService currentUserService,
        IPrincipalService authorization)
    {
        _currentUserService = currentUserService;
        _authorization = authorization;
    }

    public async Task Handle(RefreshUserInfoCommand request, CancellationToken cancellationToken)
    {
        await _authorization.CreateOrUpdateAsync(_currentUserService.User!, cancellationToken);
    }
}

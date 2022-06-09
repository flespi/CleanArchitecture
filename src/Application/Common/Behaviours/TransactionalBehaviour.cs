using CleanArchitecture.Application.Common.Transactions;
using MediatR;

namespace CleanArchitecture.Application.Common.Behaviours;

public class TransactionalBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ApplicationState _state;

    public TransactionalBehaviour(ApplicationState state)
    {
        _state = state;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        await using var trasaction = await _state.BeginTransactionAsync();

        var result = await next();
        await trasaction.CommitAsync();
        return result;
    }
}

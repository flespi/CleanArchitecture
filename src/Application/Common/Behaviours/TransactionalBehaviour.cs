using System.Reflection;
using CleanArchitecture.Application.Common.Transactions;
using MediatR;

namespace CleanArchitecture.Application.Common.Behaviours;

public class TransactionalBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly GlobalState _state;

    public TransactionalBehaviour(GlobalState state)
    {
        _state = state;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var transactionalAttributes = request.GetType().GetCustomAttributes<TransactionalAttribute>();

        if (transactionalAttributes.Any())
        {
            await using var trasaction = await _state.BeginTransactionAsync();

            var result = await next();
            await trasaction!.CommitAsync();
            return result;
        }

        return await next();
    }
}

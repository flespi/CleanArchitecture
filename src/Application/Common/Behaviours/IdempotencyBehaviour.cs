using System.Reflection;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace CleanArchitecture.Application.Common.Behaviours;

public class IdempotencyBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
{
    private readonly IIdempotentRequest _idempotency;
    private readonly IDistributedCache _cache;

    public IdempotencyBehaviour(IIdempotentRequest idempotency, IDistributedCache cache)
    {
        _idempotency = idempotency;
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var idempotentAttribute = request.GetType().GetCustomAttributes<IdempotentAttribute>();

        if (idempotentAttribute.Any())
        {
            if (_idempotency.IdempotencyKey is not null)
            {
                var response = await _cache.GetAsync<TResponse>(_idempotency.IdempotencyKey, cancellationToken);

                if (response is null)
                {
                    response = await next();
                    await _cache.SetAsync(_idempotency.IdempotencyKey!, response, cancellationToken);
                }

                return response;
            }
        }

        return await next();
    }
}

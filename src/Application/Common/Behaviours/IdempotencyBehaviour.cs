using System.Reflection;
using CleanArchitecture.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Hybrid;

namespace CleanArchitecture.Application.Common.Behaviours;

public class IdempotencyBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
{
    private readonly IIdempotentRequest _idempotency;
    private readonly HybridCache _cache;

    public IdempotencyBehaviour(IIdempotentRequest idempotency, HybridCache cache)
    {
        _idempotency = idempotency;
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var idempotentAttribute = request.GetType().GetCustomAttributes<IdempotentAttribute>();

        if (idempotentAttribute.Any())
        {
            if (_idempotency.IdempotencyKey is null)
            {
                throw new ValidationException("The idempotency key is missing.");
            }

            return await _cache.GetOrCreateAsync(
                _idempotency.IdempotencyKey,
                async cancel => await next(),
                cancellationToken: cancellationToken
            );
        }

        return await next();
    }
}

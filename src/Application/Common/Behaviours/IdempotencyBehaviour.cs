using System.Reflection;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using Microsoft.Extensions.Caching.Hybrid;

namespace CleanArchitecture.Application.Common.Behaviours;

public class IdempotencyBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
{
    private readonly IIdempotentRequest _idempotency;
    private readonly IUser _user;
    private readonly HybridCache _cache;

    public IdempotencyBehaviour(IIdempotentRequest idempotency, IUser user, HybridCache cache)
    {
        _idempotency = idempotency;
        _user = user;
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var idempotentAttribute = request.GetType().GetCustomAttributes<IdempotentAttribute>();

        if (idempotentAttribute.Any())
        {
            if (_idempotency.IdempotencyKey is null)
            {
                throw new Exceptions.ValidationException();
            }

            var entry = await _cache.GetOrCreateAsync(
                _idempotency.IdempotencyKey,
                async cancel => new UserData<TResponse>
                {
                    UserId = _user.Id,
                    Data = await next(),
                },
                cancellationToken: cancellationToken
            );

            if (entry.UserId != _user.Id)
            {
                throw new ForbiddenAccessException();
            }

            return entry.Data;
        }

        return await next();
    }
}

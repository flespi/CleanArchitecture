using CleanArchitecture.Application.Common.Types;
using MediatR;

namespace CleanArchitecture.Application.Common.Cqrs;

public abstract record CreateCommand<TData> : IRequest<Guid>, IIdempotentRequest
{
    public TData? Data { get; init; }

    public Guid? IdempotencyKey { get; init; }
}

public abstract record ReadQuery<TData> : IRequest<TData>
{
    public Guid Id { get; init; }
}

public abstract record UpdateCommand<TData> : IRequest, IConditionalRequest
{
    public Guid Id { get; init; }

    public TData? Data { get; init; }

    public Hex? ConcurrencyToken { get; init; }
}

public abstract record DeleteCommand : IRequest
{
    public Guid Id { get; init; }
}

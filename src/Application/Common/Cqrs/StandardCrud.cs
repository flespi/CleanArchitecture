using MediatR;

namespace CleanArchitecture.Application.Common.Cqrs;

public abstract record CreateCommand<TData> : IRequest<int>
{
    public TData? Data { get; init; }
}

public abstract record ReadQuery<TData> : IRequest<TData>
{
    public int Id { get; init; }
}

public abstract record UpdateCommand<TData> : IRequest
{
    public int Id { get; init; }

    public TData? Data { get; init; }
}

public abstract record DeleteCommand : IRequest
{
    public int Id { get; init; }
}

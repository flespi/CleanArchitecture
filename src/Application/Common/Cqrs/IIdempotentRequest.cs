namespace CleanArchitecture.Application.Common.Cqrs;

public interface IIdempotentRequest
{
    public Guid? IdempotencyKey { get; }
}

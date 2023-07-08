namespace CleanArchitecture.Application.Common.Interfaces;

public interface IIdempotentRequest
{
    public string? IdempotencyKey { get; }
}

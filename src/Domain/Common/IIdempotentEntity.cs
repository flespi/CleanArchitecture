namespace CleanArchitecture.Domain.Common;

public interface IIdempotentEntity
{
    Guid IdempotencyKey { get; set; }
}

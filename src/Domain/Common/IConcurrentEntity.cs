namespace CleanArchitecture.Domain.Common;

public interface IConcurrentEntity
{
    byte[]? ConcurrencyToken { get; set; }
}

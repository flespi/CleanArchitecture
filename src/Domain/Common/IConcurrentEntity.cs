using CleanArchitecture.Domain.Types;

namespace CleanArchitecture.Domain.Common;

public interface IConcurrentEntity
{
    Hex? ConcurrencyToken { get; set; }
}

using CleanArchitecture.Application.Common.Types;

namespace CleanArchitecture.Application.Common.Cqrs;

public interface IConditionalRequest
{
    Hex? ConcurrencyToken { get; }
}

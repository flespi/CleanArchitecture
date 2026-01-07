using CleanArchitecture.Domain.Types;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IConditionalParameters
{
    Hex? IfMatch { get; }
}

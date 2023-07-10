using CleanArchitecture.Application.Common.Types;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IConditionalParameters
{
    Hex? IfMatch { get; }
}

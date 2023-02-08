using CleanArchitecture.Application.Common.Types;

namespace CleanArchitecture.Application.Common.Models;

public class Versioned<TResult>
{
    public TResult Result { get; init; }

    public Hex ConcurrencyToken { get; init; }

    public Versioned(TResult result, Hex concurrencyToken)
    {
        Result = result;
        ConcurrencyToken = concurrencyToken;
    }
}

public static class Versioned
{
    public static Versioned<TResult> FromResult<TResult>(TResult result, Hex concurrencyToken)
        => new(result, concurrencyToken);
}

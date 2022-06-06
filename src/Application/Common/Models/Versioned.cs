namespace CleanArchitecture.Application.Common.Models;

public class Versioned<T>
{
    public T? Result { get; init; }

    public string? ConcurrencyToken { get; init; }
}

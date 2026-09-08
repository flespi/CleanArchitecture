namespace CleanArchitecture.Application.FunctionalTests.Internal.Abstractions;

public interface IAsyncEcoLifetime : IAsyncLifetime
{
    ValueTask ResetAsync();
}

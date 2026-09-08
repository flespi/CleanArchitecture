using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application.FunctionalTests.Internal.Abstractions;

public interface IAppTestContext : IAsyncEcoLifetime
{
    IServiceScopeFactory ServiceScopeFactory { get; }
}

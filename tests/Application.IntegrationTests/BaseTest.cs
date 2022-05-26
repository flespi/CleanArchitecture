namespace CleanArchitecture.Application.IntegrationTests;

using System.Threading.Tasks;
using Xunit;

public abstract class BaseTest : IAsyncLifetime, IClassFixture<TestContext>
{
    protected TestContext Context { get; }

    public BaseTest(TestContext context)
    {
        Context = context;
    }

    public async Task InitializeAsync()
    {
        await Context.ResetState();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}

namespace CleanArchitecture.Application.FunctionalTests;

// [Collection(BaseCollection.Name)]
public abstract class BaseTest : IClassFixture<TestContext>, IAsyncLifetime
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

namespace CleanArchitecture.Application.FunctionalTests;

using static Testing;

[TestFixture]
[SetUICulture("en")]
public abstract class BaseTestFixture
{
    [SetUp]
    public async Task TestSetUp()
    {
        await ResetState();
    }
}

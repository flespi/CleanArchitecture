using Xunit;

namespace CleanArchitecture.Application.IntegrationTests;

[CollectionDefinition(Name)]
public class BaseCollection : ICollectionFixture<TestContext>
{
    public const string Name = nameof(BaseCollection);
}

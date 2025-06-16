namespace CleanArchitecture.Application.FunctionalTests;

[CollectionDefinition(Name)]
public class BaseCollection : ICollectionFixture<TestContext>
{
    public const string Name = nameof(BaseCollection);
}

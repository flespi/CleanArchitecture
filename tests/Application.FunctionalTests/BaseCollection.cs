namespace CleanArchitecture.Application.FunctionalTests;

[CollectionDefinition(Name)]
public class BaseCollection : ICollectionFixture<AppTestContext>
{
    public const string Name = nameof(BaseCollection);
}

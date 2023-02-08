namespace CleanArchitecture.Account.Data;

public interface IDataInitialiser
{
    Task InitialiseAsync();

    Task SeedAsync();
}

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IDataInitialiser
{
    Task InitialiseAsync();

    Task SeedAsync();
}

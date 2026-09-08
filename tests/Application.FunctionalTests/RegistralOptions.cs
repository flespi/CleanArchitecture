using CleanArchitecture.Application.Common.Interfaces;
using DotNet.Testcontainers.Containers;

namespace CleanArchitecture.Application.FunctionalTests;

public class RegistralOptions
{
    public required IUser User { get; init; }

    public required IDatabaseContainer Container { get; init; }
}

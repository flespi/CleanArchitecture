using System.Data.Common;
using CleanArchitecture.Application.FunctionalTests.Internal.Abstractions;
using CleanArchitecture.Application.FunctionalTests.Internal.Data;
using CleanArchitecture.Infrastructure.Data;
using DotNet.Testcontainers.Containers;
using Respawn;

namespace CleanArchitecture.Application.FunctionalTests.Internal.Snapshot;

public class DatabaseSnapshotBuilder
{
    private RespawnerOptions? _snapshotOptions;

    public DatabaseSnapshotBuilder WithOptions(RespawnerOptions options)
    {
        _snapshotOptions = options;
        return this;
    }

    public IAsyncEcoLifetime Build(IDbConnectionFactory connectionFactory)
    {
        ArgumentNullException.ThrowIfNull(connectionFactory);

        return new DatabaseSnapshot(connectionFactory, _snapshotOptions);
    }

    public IAsyncEcoLifetime Build(IDatabaseContainer container, DbProviderFactory providerFactory)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(providerFactory);

        var connectionFactory = new ContainerDbConnectionFactory(container, providerFactory);

        return new DatabaseSnapshot(connectionFactory, _snapshotOptions);
    }
}

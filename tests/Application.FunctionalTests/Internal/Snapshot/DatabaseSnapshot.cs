using CleanArchitecture.Application.FunctionalTests.Internal.Abstractions;
using CleanArchitecture.Infrastructure.Data;
using Respawn;

namespace CleanArchitecture.Application.FunctionalTests.Internal.Snapshot;

public class DatabaseSnapshot : IAsyncEcoLifetime
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly RespawnerOptions? _snapshotOptions;

    private Respawner? _snapshot;

    public DatabaseSnapshot(
        IDbConnectionFactory connectionFactory,
        RespawnerOptions? snapshotOptions)
    {
        _connectionFactory = connectionFactory;
        _snapshotOptions = snapshotOptions;
    }

    public async ValueTask InitializeAsync()
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        _snapshot = await Respawner.CreateAsync(connection, _snapshotOptions);
    }

    public async ValueTask ResetAsync()
    {
        if (_snapshot is null) return;

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await _snapshot.ResetAsync(connection);
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}

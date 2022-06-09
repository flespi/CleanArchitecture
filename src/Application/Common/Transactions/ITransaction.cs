namespace CleanArchitecture.Application.Common.Transactions;

public interface ITransaction : IAsyncDisposable
{
    Task CommitAsync();

    Task RollbackAsync();

    async ValueTask IAsyncDisposable.DisposeAsync() => await RollbackAsync();
}

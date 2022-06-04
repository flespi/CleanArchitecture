using CleanArchitecture.Application.Common.Exceptions;

namespace CleanArchitecture.Application.Common.Transactions;

public abstract class Transaction : ITransaction
{
    private readonly SemaphoreSlim _semaphore = new (1);

    private bool _completed = false;

    protected abstract Task ExecuteCommitAsync();

    protected abstract Task ExecuteRollbackAsync();

    public async Task CommitAsync()
    {
        await _semaphore.WaitAsync();

        if (_completed)
        {
            throw new TransactionCompletedException();
        }

        await ExecuteCommitAsync();

        _completed = true;

        _semaphore.Release();
    }

    public async Task RollbackAsync()
    {
        await _semaphore.WaitAsync();

        if (_completed)
        {
            throw new TransactionCompletedException();
        }

        await ExecuteRollbackAsync();

        _completed = true;

        _semaphore.Release();
    }

    public async ValueTask DisposeAsync()
    {
        if (!_completed)
        {
            await RollbackAsync();
        }
    }
}

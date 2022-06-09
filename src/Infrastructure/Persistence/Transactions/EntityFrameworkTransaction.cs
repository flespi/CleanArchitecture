using CleanArchitecture.Application.Common.Transactions;
using Microsoft.EntityFrameworkCore.Storage;

namespace CleanArchitecture.Infrastructure.Persistence.Transactions;

public class EntityFrameworkTransaction : Transaction
{
    private readonly IDbContextTransaction _transaction;

    public EntityFrameworkTransaction(IDbContextTransaction transaction)
    {
        _transaction = transaction;
    }

    protected override async Task ExecuteCommitAsync()
    {
        await _transaction.CommitAsync();
    }

    protected override async Task ExecuteRollbackAsync()
    {
        await _transaction.RollbackAsync();
    }
}

namespace CleanArchitecture.Application.Common.Transactions;

public class ApplicationStateTransaction : Transaction
{
    private readonly IEnumerable<ITransaction> _transactions;

    public ApplicationStateTransaction(IEnumerable<ITransaction> transactions)
    {
        _transactions = transactions;
    }

    protected override async Task ExecuteCommitAsync()
    {
        foreach (var transaction in _transactions)
        {
            await transaction.CommitAsync();
        }
    }

    protected override async Task ExecuteRollbackAsync()
    {
        foreach (var transaction in _transactions)
        {
            await transaction.RollbackAsync();
        }
    }
}

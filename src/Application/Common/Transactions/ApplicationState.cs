namespace CleanArchitecture.Application.Common.Transactions;

public class ApplicationState : ITransactional
{
    private readonly IEnumerable<ITransactional> _transactionals;

    public ApplicationState(IEnumerable<ITransactional> transactionals)
    {
        _transactionals = transactionals;
    }

    public async Task<ITransaction?> BeginTransactionAsync()
    {
        var transactions = await CreateTransactions().ToListAsync();
        return new ApplicationStateTransaction(transactions);

        async IAsyncEnumerable<ITransaction> CreateTransactions()
        {
            foreach (var transactional in _transactionals)
            {
                var transaction = await transactional.BeginTransactionAsync();

                if (transaction is not null)
                {
                    yield return transaction;
                }
            }
        }
    }
}

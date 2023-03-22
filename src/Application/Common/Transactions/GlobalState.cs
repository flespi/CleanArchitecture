namespace CleanArchitecture.Application.Common.Transactions;

public class GlobalState : ITransactional
{
    private readonly IEnumerable<ITransactional> _transactionals;

    public GlobalState(IEnumerable<ITransactional> transactionals)
    {
        _transactionals = transactionals;
    }

    public async Task<ITransaction> BeginTransactionAsync()
    {
        var transactions = await CreateTransactions().ToListAsync();
        return new GlobalTransaction(transactions);

        async IAsyncEnumerable<ITransaction> CreateTransactions()
        {
            foreach (var transactional in _transactionals)
            {
                yield return await transactional.BeginTransactionAsync();
            }
        }
    }
}

namespace CleanArchitecture.Application.Common.Transactions;

public interface ITransactional
{
    Task<ITransaction> BeginTransactionAsync();
}

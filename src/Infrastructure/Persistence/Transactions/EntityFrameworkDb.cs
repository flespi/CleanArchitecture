using CleanArchitecture.Application.Common.Transactions;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence.Transactions;

public class EntityFrameworkDb<TContext> : EntityFrameworkDb
    where TContext : DbContext
{
    public EntityFrameworkDb(TContext context) : base(context)
    {
    }
}

public class EntityFrameworkDb : ITransactional
{
    private readonly DbContext _context;

    public EntityFrameworkDb(DbContext context)
    {
        _context = context;
    }

    public async Task<ITransaction?> BeginTransactionAsync()
    {
        if (_context.Database.IsInMemory()) return null;

        var transaction = await _context.Database.BeginTransactionAsync();
        return new EntityFrameworkTransaction(transaction);
    }
}

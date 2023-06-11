namespace Master.Infrastructure.EFCore.Common;

using Command;
using Core.Contract.Infrastructure.Common;
using Master.Core.Contract.Infrastructure.Command;

public class UnitOfWork<TContext> : IUnitOfWork where TContext : CommandDbContext
{
    private readonly TContext _context;

    public UnitOfWork(TContext context) => _context = context;

    public string TransactionalAction(Action action) => _context.TransactionalAction(action);
    public async Task<String> TransactionalActionAsync(Action action) => await _context.TransactionalActionAsync(action);
}

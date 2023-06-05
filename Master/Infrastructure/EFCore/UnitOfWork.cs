namespace Master.Infrastructure.EFCore;

using Command;
using Core.Contract.Infrastructure.Common;

public class UnitOfWork<TContext> : IDisposable, IUnitOfWork where TContext : CommandDbContext
{
    private readonly TContext _context;

    public UnitOfWork(TContext context) => _context = context;

    public void BeginTransaction() => _context.BeginTransaction();
    public async Task BeginTransactionAsync() => await _context.BeginTransactionAsync();

    public void Commit() => _context.Commit();
    public async Task CommitAsync() => await _context.CommitAsync();

    public void Rollback() => _context.Rollback();
    public async Task RollbackAsync() => await _context.RollbackAsync();

    public void Save() => _context.SaveChanges();
    public void Save(bool acceptAllChangesOnSuccess) => _context.SaveChanges(acceptAllChangesOnSuccess);

    public async Task SaveAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
    public async Task SaveAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

    public void TransactionalAction(Action action)
    {
        try
        {
            if (action is not null)
            {
                BeginTransaction();
                action.Invoke();
                Save();
                Commit();
            }
        }
        catch (Exception)
        {

            Rollback();
        }
        finally
        {
            //Dispose();
        }
    }

    public async Task TransactionalActionAsync(Action action)
    {
        try
        {
            if (action is not null)
            {
                await BeginTransactionAsync();
                action.Invoke();
                await SaveAsync();
                await CommitAsync();
            }
        }
        catch (Exception)
        {

            await RollbackAsync();
        }
        finally
        {
            //Dispose();
        }
    }

    public void Dispose() =>
          _context.Dispose();
}

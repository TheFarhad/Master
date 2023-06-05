namespace Master.Infrastructure.EFCore.Command;

using Master.Utilities.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;

public class CommandDbContext : DbContext
{
    private IDbContextTransaction _transaction;

    public CommandDbContext(DbContextOptions options) : base(options) { }



    public override int SaveChanges()
    {
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public void BeginTransaction() =>
        _transaction = Database.BeginTransaction();

    public async Task BeginTransactionAsync() =>
       _transaction = await Database.BeginTransactionAsync();

    public void Commit()
    {
        ValidateTransaction();
        _transaction.Commit();
    }

    public async Task CommitAsync()
    {
        ValidateTransaction();
        await _transaction.CommitAsync();
    }

    public void Rollback()
    {
        ValidateTransaction();
        _transaction.Rollback();
    }

    public async Task RollbackAsync()
    {
        ValidateTransaction();
        await _transaction.RollbackAsync();
    }

    private void ValidateTransaction()
    {
        if (_transaction.IsNull())
            throw new NullReferenceException("Please call `BeginTransaction()` method first.");
    }
}

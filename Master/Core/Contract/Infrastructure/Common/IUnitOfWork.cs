namespace Master.Core.Contract.Infrastructure.Common;

public interface IUnitOfWork
{
    void BeginTransaction();
    Task BeginTransactionAsync();
    void Commit();
    Task CommitAsync();
    void Rollback();
    Task RollbackAsync();
    void Save();
    void Save(bool acceptAllChangesOnSuccess);
    Task SaveAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
    void TransactionalAction(Action action);
    Task TransactionalActionAsync(Action action);
}

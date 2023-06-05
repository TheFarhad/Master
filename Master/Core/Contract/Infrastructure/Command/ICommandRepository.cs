namespace Master.Core.Contract.Infrastructure.Command;

using System.Linq.Expressions;
using Domain.Aggregate.Entity;
using Domain.Aggregate.ValueObject;

public interface ICommandRepository<TEntity> where TEntity : AggregateRoot
{
    void Add(TEntity source);
    Task AddAsync(TEntity source);
    void BulkAdd(IEnumerable<TEntity> source);
    Task BulkAddAsync(IEnumerable<TEntity> source);
    TEntity? Get(long id);
    TEntity? GetGraph(long id);
    Task<TEntity>? GetAsync(long id);
    Task<TEntity>? GetGrapthAsync(long id);
    TEntity? Get(Code code);
    TEntity GetGraph(Code code);
    Task<TEntity>? GetAsync(Code code);
    Task<TEntity>? GetGraphAsync(Code code);
    TEntity? Get(Expression<Func<TEntity, bool>> expression);
    Task<TEntity>? GetAsync(Expression<Func<TEntity, bool>> expression);
    bool Any(Expression<Func<TEntity, bool>> expression);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);
    void Remove(long id);
    void RemoveGraph(long id);
    void Remove(TEntity source);
    void Save();
    Task SaveAsync();
    string TransactionalAction(Action action);
    Task<string> TransactionalActionAsync(Action action);
}

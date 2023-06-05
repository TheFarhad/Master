namespace Master.Infrastructure.EFCore.Command;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Core.Domain.Aggregate.Entity;
using Core.Contract.Infrastructure.Command;
using Core.Domain.Aggregate.ValueObject;

public class CommandRepository<TEntity, TContext> : ICommandRepository<TEntity> where TEntity : AggregateRoot where TContext : CommandDbContext
{
    protected readonly TContext _context;

    public CommandRepository(TContext context) => _context = context;

    public void Add(TEntity source) => _context.Add(source);
    public async Task AddAsync(TEntity source) => await _context.AddAsync(source);

    public bool Any(Expression<Func<TEntity, bool>> expression) => Set().Any(expression);
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression) => await Set().AnyAsync(expression);

    public void BulkAdd(IEnumerable<TEntity> source) => _context.AddRange(source);
    public Task BulkAddAsync(IEnumerable<TEntity> source) => _context.AddRangeAsync(source);

    public TEntity? Get(long id) => Set().Find(id);
    public TEntity? Get(Code code) => Set().FirstOrDefault(_ => _.Code.Value == code.Value);
    public TEntity? Get(Expression<Func<TEntity, bool>> expression) => Set().FirstOrDefault(expression);
    public async Task<TEntity>? GetAsync(long id) => await Set().FindAsync(id);
    public async Task<TEntity>? GetAsync(Code code) => await Set().FirstOrDefaultAsync(_ => _.Code.Value == code.Value);
    public async Task<TEntity>? GetAsync(Expression<Func<TEntity, bool>> expression) => await Set().FirstOrDefaultAsync(expression);

    public TEntity GetGraph(long id)
    {
        throw new NotImplementedException();
    }

    public TEntity GetGraph(Code code)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetGraphAsync(Code code)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetGrapthAsync(long id)
    {
        throw new NotImplementedException();
    }

    public void Remove(TEntity source) => Set().Remove(source);
    public void Remove(long id)
    {
        var entity = Get(id);
        if (entity is not null) Remove(entity);
    }
    public void RemoveGraph(long id)
    {
        var entity = GetGraph(id);
        if (entity is not null) Remove(entity);
    }

    public void Save() => _context.SaveChanges();
    public async Task SaveAsync() => await _context.SaveChangesAsync();

    public string TransactionalAction(Action action) => _context.TransactionalAction(action);
    public async Task<string> TransactionalActionAsync(Action action) => await _context.TransactionalActionAsync(action);

    #region utilities

    protected DbSet<TEntity> Set() => _context.Set<TEntity>();

    private IEnumerable<string> Relations() => _context.Relations(typeof(TEntity));

    #endregion
}
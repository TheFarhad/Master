namespace Master.Infrastructure.EFCore.Command;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Domain.Aggregate.Entity;
using Core.Contract.Infrastructure.Command;
using Core.Domain.Aggregate.ValueObject;

public class CommandRepository<TEntity, TContext> : ICommandRepository<TEntity> where TEntity : AggregateRoot where TContext : CommandDbContext
{
    protected readonly TContext _context;

    public CommandRepository(TContext context) => _context = context;

    public void Add(TEntity source)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(TEntity source)
    {
        throw new NotImplementedException();
    }

    public bool Any(Expression<Func<TEntity, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public void BulkAdd(IEnumerable<TEntity> source)
    {
        throw new NotImplementedException();
    }

    public Task BulkAddAsync(IEnumerable<TEntity> source)
    {
        throw new NotImplementedException();
    }

    public TEntity Get(long id)
    {
        throw new NotImplementedException();
    }

    public TEntity Get(Code code)
    {
        throw new NotImplementedException();
    }

    public TEntity Get(Expression<Func<TEntity, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetAsync(Code code)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
    {
        throw new NotImplementedException();
    }

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

    public void Remove(long id)
    {
        throw new NotImplementedException();
    }

    public void Remove(TEntity source)
    {
        throw new NotImplementedException();
    }

    public void RemoveGraph(long id)
    {
        throw new NotImplementedException();
    }

    public void Save() => _context.SaveChanges();
    public async Task SaveAsync() => await _context.SaveChangesAsync();
}
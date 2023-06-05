﻿namespace Master.Infrastructure.EFCore;

using Command;
using Core.Contract.Infrastructure.Common;

public class UnitOfWork<TContext> : IUnitOfWork where TContext : CommandDbContext
{
    private readonly TContext _context;

    public UnitOfWork(TContext context) => _context = context;

    public string TransactionalAction(Action action) => _context.TransactionalAction(action);
    public async Task<String> TransactionalActionAsync(Action action) => await _context.TransactionalActionAsync(action);
}

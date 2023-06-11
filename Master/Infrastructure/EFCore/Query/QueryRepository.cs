namespace Master.Infrastructure.EFCore.Query;

using Master.Core.Contract.Infrastructure.Query;

public class QueryRepository<TContext> : IQueryRepository where TContext : QueryDbContext
{
    protected readonly TContext Context;

    public QueryRepository(TContext context) => Context = context;
}

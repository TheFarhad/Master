namespace Master.Core.Application.Query;

using Contract.Application.Query;

public abstract class QueryDispatcherDecorator : IQueryDispatcher
{
    protected IQueryDispatcher Dispatcher;
    public abstract int Order { get; }

    public void Set(IQueryDispatcher dispatcher) => Dispatcher = dispatcher;

    public abstract Task<QueryResult<TPayload>> DispatchAsync<TQuery, TPayload>(TQuery source) where TQuery : IQuery<TPayload>;
}
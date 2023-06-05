namespace Master.Core.Contract.Application.Query;

public interface IQueryDispatcher
{
    Task<QueryResult<TData>> DispatchAsync<TQuery, TData>(TQuery source) where TQuery : IQuery<TData>;
}

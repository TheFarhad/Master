namespace Master.Core.Contract.Application.Query;

public interface IQueryDispatcher
{
    Task<QueryResult<TPayload>> DispatchAsync<TQuery, TPayload>(TQuery source) where TQuery : IQuery<TPayload>;
}

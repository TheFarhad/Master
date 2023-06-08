namespace Master.Core.Contract.Application.Query;

public interface IQueryHandler<TQuery, TPayload> where TQuery : IQuery<TPayload>
{
    Task<QueryResult<TPayload>> HandleAsync(TQuery source);
}

namespace Master.Core.Contract.Application.Query;

public interface IQueryHandler<TQuery, TData> where TQuery : IQuery<TData>
{
    Task<QueryResult<TData>> HandleAsync(TQuery source);
}

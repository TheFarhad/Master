namespace Master.Core.Application.Query;

using Contract.Application.Query;
using Contract.Application.Common;
using Utilities.Extentions;

public abstract class QueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    protected QueryResult<TResult> Result = new();

    public abstract Task<QueryResult<TResult>> HandleAsync(TQuery source);

    protected Task<QueryResult<TResult>> OK(TResult payload)
    {
        Result.Payload = payload;
        return Task.FromResult(Result);
    }

    protected Task<QueryResult<TResult>> NotFound()
    {
        Result.SetError("");
        Result.Status = ServiceStatus.NotFound;
        Result.Payload = default;
        return Task.FromResult(Result);
    }

    protected Task<QueryResult<TResult>> ResultAsync(TResult payload, ServiceStatus status, string error = null)
    {
        if (!error.IsNull()) Result.SetError(error);
        Result.Status = status;
        Result.Payload = payload;
        return Task.FromResult(Result);
    }
}

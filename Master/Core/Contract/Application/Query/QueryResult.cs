namespace Master.Core.Contract.Application.Query;

using Common;

public class QueryResult<TData> : ServiceResult
{
    public TData? Payload { get; set; }
}

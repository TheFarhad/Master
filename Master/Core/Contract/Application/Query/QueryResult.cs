namespace Master.Core.Contract.Application.Query;

using Common;

public class QueryResult<TPayload> : ServiceResult
{
    public TPayload? Payload { get; set; }
}

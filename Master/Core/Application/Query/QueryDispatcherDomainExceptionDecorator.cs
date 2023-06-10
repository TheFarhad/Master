namespace Master.Core.Application.Query;

using Microsoft.Extensions.Logging;
using Utilities.Extentions;
using Contract.Application.Query;
using Core.Domain.Aggregate.Exception;

public class QueryDispatcherDomainExceptionDecorator : QueryDispatcherDecorator
{
    protected override int Order => 2;
    private readonly ILogger<QueryDispatcherDomainExceptionDecorator> _logger;

    public QueryDispatcherDomainExceptionDecorator(ILogger<QueryDispatcherDomainExceptionDecorator> logger) =>
        _logger = logger;

    public override async Task<QueryResult<TPayload>> DispatchAsync<TQuery, TPayload>(TQuery source)
    {
        var result = default(QueryResult<TPayload>);
        var type = source.Type();
        try
        {
            result = await DispatchAsync<TQuery, TPayload>(source);
        }
        catch (DomainStateException e)
        {
            result = DomainExceptionCatchResult<TQuery, TPayload>(source, type, e);
        }
        catch (AggregateException e)
        {
            if (e.InnerException is DomainStateException ex)
                result = DomainExceptionCatchResult<TQuery, TPayload>(source, type, ex);
            else
                throw e;
        }
        return result;
    }

    private QueryResult<TPayload> DomainExceptionCatchResult<TQuery, TPayload>(TQuery source, Type queryType, DomainStateException exception)
    {
        LogError(source, queryType, exception);
        return DomainExceptionHandlingWithReturnValue<TPayload>(exception);
    }

    private void LogError<TQuery>(TQuery source, Type queryType, DomainStateException e)
    {
        _logger.LogError(e, "Processing of {QueryType} With value {Query} failed at {StartDateTime} because there are domain exceptions.", queryType, source, DateTime.Now);
    }

    private QueryResult<TPaylod> DomainExceptionHandlingWithReturnValue<TPaylod>(DomainStateException exception)
    {
        var result = new QueryResult<TPaylod>
        {
            Status = Contract.Application.Common.ServiceStatus.InvalidDomainState,
        };
        result.SetError(GetExceptionText(exception));
        return result;
    }

    private string GetExceptionText(DomainStateException domainStateException)
    {
        var result = domainStateException.ToString();
        _logger.LogInformation("Domain Exception message is {DomainExceptionMessage}", result);
        return result;
    }
}
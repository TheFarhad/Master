namespace Master.Core.Application.Query;

using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Utilities.Extentions;
using Contract.Application.Query;

public class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<QueryDispatcher> _logger;
    private Stopwatch _timer;

    public QueryDispatcher(IServiceProvider provider, ILogger<QueryDispatcher> logger)
    {
        _provider = provider;
        _logger = logger;
        _timer = new Stopwatch();
    }

    public async Task<QueryResult<TPayload>> DispatchAsync<TQuery, TPayload>(TQuery source) where TQuery : IQuery<TPayload>
    {
        var type = source.Type();
        _timer.Start();
        try
        {
            _logger.LogDebug("Routing command of type {QueryType} With value {Query}  Start at {StartDateTime}", type, source, DateTime.Now);

            return await _provider
                .GetRequiredService<IQueryHandler<TQuery, TPayload>>()
                .HandleAsync(source);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "There is not suitable handler for {QueryType} Routing failed at {StartDateTime}.", type, DateTime.Now);
            throw;
        }
        finally
        {
            _timer.Stop();
            _logger.LogInformation("Processing the {QueryType} query tooks {Millisecconds} Millisecconds", type, _timer.ElapsedMilliseconds);
        }
    }
}
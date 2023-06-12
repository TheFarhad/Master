namespace Master.Core.Application.Event;

using Microsoft.Extensions.Logging;
using Utilities.Extentions;
using Domain.Aggregate.Exception;

public class EventDispatcherDomainExceptionDecorator : EventDispatcherDecorator
{
    public override int Order => 2;
    private readonly ILogger<EventDispatcherDomainExceptionDecorator> _logger;

    public EventDispatcherDomainExceptionDecorator(ILogger<EventDispatcherDomainExceptionDecorator> logger) =>
        _logger = logger;

    public override async Task DispatchAsync<TEvent>(TEvent source)
    {
        var type = source.Type();
        try
        {
            await Dispatcher.DispatchAsync(source);
        }
        catch (DomainStateException e)
        {
            LogError(source, type, e);
        }
        catch (AggregateException e)
        {
            if (e.InnerException is DomainStateException ie)
                LogError(source, type, ie);

            throw e;
        }
    }

    private void LogError<TEvent>(TEvent source, Type @event, Exception exception)
    {
        _logger.LogError(exception, "Processing of {EventType} With value {Event} failed at {StartDateTime} because there are domain exceptions.", @event, source, DateTime.Now);
    }
}

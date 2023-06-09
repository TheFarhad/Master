namespace Master.Core.Application.Command;

using Microsoft.Extensions.Logging;
using Utilities.Extentions;
using Contract.Application.Command;
using Domain.Aggregate.Exception;
using Contract.Application.Common;

public class CommandDispacherDomainExceptionDecorator : CommandDispacherDecorator
{
    protected override int Order => 2;
    private readonly ILogger<CommandDispacherDomainExceptionDecorator> _logger;

    public CommandDispacherDomainExceptionDecorator(ILogger<CommandDispacherDomainExceptionDecorator> logger) =>
          _logger = logger;

    public override async Task<CommandResult> DispatchAsync<TCommand>(TCommand source)
    {
        var type = CommandType(source);
        try
        {
            return await Dispatcher.DispatchAsync<TCommand>(source);
        }
        catch (DomainStateException e)
        {
            LogError(source, type, e);
            return DomainExceptionHandlingWithoutReturnValue(e);
        }
        catch (AggregateException e)
        {
            if (e.InnerException is DomainStateException ex)
            {
                LogError(source, type, e);
                return DomainExceptionHandlingWithoutReturnValue(ex);
            }
            throw;
        }
    }

    public override async Task<CommandResult<TPayload>> DispatchAsync<TCommand, TPayload>(TCommand source)
    {
        var type = CommandType(source);
        try
        {
            return await Dispatcher.DispatchAsync<TCommand, TPayload>(source);
        }
        catch (DomainStateException e)
        {
            LogError(source, type, e);
            return DomainExceptionHandlingWithoutReturnValue<TPayload>(e);
        }
        catch (AggregateException e)
        {
            if (e.InnerException is DomainStateException ex)
            {
                LogError(source, type, ex);
                return DomainExceptionHandlingWithoutReturnValue<TPayload>(ex);
            }
            throw e;
        }
    }

    private Type CommandType<TCommand>(TCommand source) => source.Type();

    private void LogError<TCommand>(TCommand source, Type command, Exception exception)
    {
        _logger.LogError(exception, "Processing of {CommandType} With value {Command} failed at {StartDateTime} because there are domain exceptions.", command, source, DateTime.Now);
    }

    private CommandResult DomainExceptionHandlingWithoutReturnValue(DomainStateException ex)
    {
        var result = new CommandResult
        {
            Status = ServiceStatus.InvalidDomainState
        };
        result.SetError(GetExceptionText(ex));
        return result;
    }

    private CommandResult<TPayload> DomainExceptionHandlingWithoutReturnValue<TPayload>(DomainStateException ex)
    {
        var result = new CommandResult<TPayload>
        {
            Status = ServiceStatus.InvalidDomainState
        };
        result.SetError(GetExceptionText(ex));
        return result;
    }

    private string GetExceptionText(DomainStateException source)
    {
        var result = String.Empty;

        //var translator = _serviceProvider.GetService<ITranslator>();
        //if (translator == null)
        //    return source.ToString();

        //var result = (source?.Parameters.Any() == true) ?
        //     translator[source.Message, source.Parameters] :
        //       translator[source?.Message];

        _logger.LogInformation("Domain Exception message is {DomainExceptionMessage}", result);

        return result;
    }
}
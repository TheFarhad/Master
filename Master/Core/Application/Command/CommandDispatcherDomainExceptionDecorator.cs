namespace Master.Core.Application.Command;

using Microsoft.Extensions.Logging;
using Utilities.Extentions;
using Contract.Application.Command;
using Domain.Aggregate.Exception;
using Contract.Application.Common;

public class CommandDispatcherDomainExceptionDecorator : CommandDispatcherDecorator
{
    public override int Order => 2;
    private readonly ILogger<CommandDispatcherDomainExceptionDecorator> _logger;

    public CommandDispatcherDomainExceptionDecorator(ILogger<CommandDispatcherDomainExceptionDecorator> logger) =>
          _logger = logger;

    public override async Task<CommandResult> DispatchAsync<TCommand>(TCommand source)
    {
        var result = default(CommandResult);
        var type = CommandType(source);
        try
        {
            result = await Dispatcher.DispatchAsync<TCommand>(source);
        }
        catch (DomainStateException e)
        {
            return DomainExceptionCatchResult<TCommand>(source, type, e);
        }
        catch (AggregateException e)
        {
            if (e.InnerException is DomainStateException ex)
                return DomainExceptionCatchResult<TCommand>(source, type, ex);
            else
                throw;
        }
        return result;
    }

    public override async Task<CommandResult<TPayload>> DispatchAsync<TCommand, TPayload>(TCommand source)
    {
        var result = default(CommandResult<TPayload>);
        var type = CommandType(source);
        try
        {
            result = await Dispatcher.DispatchAsync<TCommand, TPayload>(source);
        }
        catch (DomainStateException e)
        {
            return DomainExceptionCatchResult<TCommand, TPayload>(source, type, e);
        }
        catch (AggregateException e)
        {
            if (e.InnerException is DomainStateException ex)
                return DomainExceptionCatchResult<TCommand, TPayload>(source, type, ex);
            else
                throw e;
        }
        return result;
    }

    #region utilities

    private CommandResult DomainExceptionCatchResult<TCommand>(TCommand source, Type commandType, DomainStateException exception)
    {
        LogError(source, commandType, exception);
        return DomainExceptionHandlingWithoutReturnValue(exception);
    }

    private CommandResult<TPayload> DomainExceptionCatchResult<TCommand, TPayload>(TCommand source, Type commandType, DomainStateException exception)
    {
        LogError(source, commandType, exception);
        return DomainExceptionHandlingWithoutReturnValue<TPayload>(exception);
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
        var result = source.ToString();
        _logger.LogInformation("Domain Exception message is {DomainExceptionMessage}", result);
        return result;
    }

    #endregion
}
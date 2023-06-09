namespace Master.Core.Application.Command;

using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Contract.Application.Command;
using Utilities.Extentions;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<CommandDispatcher> _logger;
    private Stopwatch _timer;

    public CommandDispatcher(IServiceProvider provider, ILogger<CommandDispatcher> logger)
    {
        _provider = provider;
        _logger = logger;
        _timer = new Stopwatch();
    }

    public async Task<CommandResult> DispatchAsync<TCommand>(TCommand source) where TCommand : ICommand
    {
        var type = CommandType(source);
        _timer.Start();
        try
        {
            LogStart(source, type);
            return await _provider
                .GetRequiredService<ICommandHandler<TCommand>>()
                .HandleAsync(source);
        }
        catch (Exception e)
        {
            LogError(e, type);
            throw;
        }
        finally
        {
            _timer.Stop();
            LogFinal(type);
        }
    }

    public async Task<CommandResult<TPayload>> DispatchAsync<TCommand, TPayload>(TCommand source) where TCommand : ICommand<TPayload>
    {
        var type = CommandType(source);
        _timer.Start();
        try
        {
            LogStart(source, type);
            return await _provider
                .GetRequiredService<ICommandHandler<TCommand, TPayload>>()
                .HandleAsync(source);
        }
        catch (Exception e)
        {
            LogError(e, type);
            throw;
        }
        finally
        {
            _timer.Stop();
            LogFinal(type);
        }
    }

    private Type CommandType<TCommand>(TCommand Source) => Source.Type();

    private void LogStart<TCommand>(TCommand source, Type commandType) =>
           _logger.LogDebug("Routing command of type {CommandType} With value {Command}  Start at {StartDateTime}", commandType, source, DateTime.Now);

    private void LogError(Exception exception, Type commandType) =>
         _logger.LogError(exception, "There is not suitable handler for {CommandType} Routing failed at {StartDateTime}.", commandType, DateTime.Now);

    private void LogFinal(Type commandType) =>
         _logger.LogInformation("Processing the {CommandType} command tooks {Millisecconds} Millisecconds", commandType, _timer.ElapsedMilliseconds);
}

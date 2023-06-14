namespace Master.Core.Application.Command;

using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Utilities.Extentions;
using Contract.Application.Command;
using Contract.Application.Common;

public class CommandDispatcherValidationDecorator : CommandDispatcherDecorator
{
    public override int Order => 1;
    private readonly IServiceProvider _service;
    private readonly ILogger<CommandDispatcherValidationDecorator> _logger;

    public CommandDispatcherValidationDecorator(ILogger<CommandDispatcherValidationDecorator> logger, IServiceProvider service)
    {
        _logger = logger;
        _service = service;
    }

    public override async Task<CommandResult> DispatchAsync<TCommand>(TCommand source)
    {
        var result = default(CommandResult);
        var type = CommandType(source);

        LogStart(source, type);

        var validationResult = Validate<TCommand, CommandResult>(source);
        if (validationResult?.Errors.Any() == true)
        {
            LogError(source, type, validationResult.Errors);
            result = await Task.FromResult(validationResult);
        }

        LogSuccess(source, type);
        result = await DispatchAsync<TCommand>(source);
        return result;
    }

    public override async Task<CommandResult<TPayload>> DispatchAsync<TCommand, TPayload>(TCommand source)
    {
        var result = default(CommandResult<TPayload>);
        var type = CommandType(source);

        LogStart(source, type);

        var validationResult = Validate<TCommand, CommandResult<TPayload>>(source);
        if (validationResult?.Errors.Any() == true)
        {
            LogError(source, type, validationResult.Errors);
            result = await Task.FromResult(validationResult);
        }

        LogSuccess(source, type);
        result = await DispatchAsync<TCommand, TPayload>(source);
        return result;
    }

    private Type CommandType<TCommand>(TCommand source) => source.Type();

    private void LogStart<TCommand>(TCommand source, Type command) =>
            _logger.LogDebug("Validating command of type {CommandType} With value {Command}  start at :{StartDateTime}", command, source, DateTime.Now);

    private void LogError<TCommand>(TCommand source, Type command, IEnumerable<string> errors) =>
          _logger.LogInformation("Validating command of type {CommandType} With value {Command}  failed. Validation errors are: {ValidationErrors}", command, source, errors);

    private void LogSuccess<TCommand>(TCommand source, Type command) =>
        _logger.LogDebug("Validating command of type {CommandType} With value {Command}  finished at :{EndDateTime}", command, source, DateTime.Now);

    private TServiceResult? Validate<TCommand, TServiceResult>(TCommand source) where TServiceResult : ServiceResult, new()
    {
        TServiceResult result = default;
        var validator = _service.GetService<IValidator<TCommand>>();
        if (validator.IsNotNull())
        {
            var validationResult = validator.Validate(source);
            if (!validationResult.IsValid)
            {
                result = new() { Status = ServiceStatus.ValidationError };
                foreach (var item in validationResult.Errors)
                    result.SetError(item.ErrorMessage);
            }
        }
        else
            _logger.LogInformation("There is not any validator for {CommandType}", CommandType(source));

        return result;
    }
}

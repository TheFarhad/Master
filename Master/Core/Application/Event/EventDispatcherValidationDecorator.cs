namespace Master.Core.Application.Event;

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Utilities.Extentions;

public class EventDispatcherValidationDecorator : EventDispatcherDecorator
{
    public override int Order => 1;
    private readonly IServiceProvider _provider;
    private readonly ILogger<EventDispatcherValidationDecorator> _logger;

    public EventDispatcherValidationDecorator(ILogger<EventDispatcherValidationDecorator> logger, IServiceProvider provider)
    {
        _logger = logger;
        _provider = provider;
    }

    public override async Task DispatchAsync<TEvent>(TEvent source)
    {
        var type = source.Type();
        _logger.LogDebug("Validating Event of type {EventType} With value {Event}  start at :{StartDateTime}", type, source, DateTime.Now);

        var errors = Validate(source);

        if (errors.Any())
        {
            _logger.LogInformation("Validating query of type {QueryType} With value {Query}  failed. Validation errors are: {ValidationErrors}", source, source, errors);
        }
        else
        {
            _logger.LogDebug("Validating query of type {QueryType} With value {Query}  finished at :{EndDateTime}", source, source, DateTime.Now);

            await Dispatcher.DispatchAsync(source);
        }
    }

    private IEnumerable<string> Validate<TEvent>(TEvent source)
    {
        var result = new List<string>();
        var validator = _provider.GetService<IValidator<TEvent>>();

        if (validator.IsNotNull())
        {
            var validationResult = validator.Validate(source);
            if (!validationResult.IsValid)
                result = validationResult
                    .Errors
                    .Select(c => c.ErrorMessage).
                    ToList();
        }
        else
            _logger.LogInformation("There is not any validator for {EventType}", source.Type());

        return result;
    }
}

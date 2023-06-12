namespace Master.Core.Application.Query;

using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Utilities.Extentions;
using Contract.Application.Query;
using Contract.Application.Common;

public class QueryDispatcherValidationDecorator : QueryDispatcherDecorator
{
    public override int Order => 1;
    private readonly IServiceProvider _service;
    private readonly ILogger<QueryDispatcherValidationDecorator> _logger;

    public QueryDispatcherValidationDecorator(ILogger<QueryDispatcherValidationDecorator> logger, IServiceProvider service)
    {
        _logger = logger;
        _service = service;
    }

    public override async Task<QueryResult<TPayload>> DispatchAsync<TQuery, TPayload>(TQuery source)
    {
        var result = new QueryResult<TPayload>();
        var type = source.Type();

        _logger.LogDebug("Validating query of type {QueryType} With value {Query}  start at :{StartDateTime}", type, source, DateTime.Now);

        var validationResult = Validate<TQuery, TPayload>(source);
        if (validationResult?.Errors.Any() == true)
        {
            _logger.LogInformation("Validating query of type {QueryType} With value {Query}  failed. Validation errors are: {ValidationErrors}", type, source, validationResult.Errors);
            result = validationResult;
        }

        _logger.LogDebug("Validating query of type {QueryType} With value {Query}  finished at :{EndDateTime}", type, source, DateTime.Now);

        result = await DispatchAsync<TQuery, TPayload>(source);
        return result;
    }

    private QueryResult<TPayload> Validate<TQuery, TPayload>(TQuery source)
    {
        var result = default(QueryResult<TPayload>);
        var validator = _service.GetRequiredService<IValidator<TQuery>>();
        if (validator.IsNotNull())
        {
            var validationResult = validator.Validate(source);
            if (!validationResult.IsValid)
            {
                result = new QueryResult<TPayload> { Status = ServiceStatus.ValidationError };
                foreach (var item in validationResult.Errors)
                    result.SetError(item.ErrorMessage);
            }
        }
        else
            _logger.LogInformation("There is not any validator for {QueryType}", source.Type());

        return result;
    }
}

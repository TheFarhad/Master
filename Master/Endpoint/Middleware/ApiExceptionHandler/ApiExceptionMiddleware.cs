namespace Master.Endpoint.Middleware.ApiExceptionHandler;

using Utilities.Services.Abstraction.Serializing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Utilities.Extentions;
using System.Net;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ISerialize _serializer;
    private readonly ILogger<ApiExceptionMiddleware> _logger;
    private readonly ApiExceptionOption _options;

    public ApiExceptionMiddleware(RequestDelegate next, ApiExceptionOption options, ILogger<ApiExceptionMiddleware> logger, ISerialize serializer)
    {
        _next = next;
        _options = options;
        _logger = logger;
        _serializer = serializer;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {

        var error = new ApiError
        {
            Id = Guid.NewGuid().ToString(),
            Status = (short)HttpStatusCode.InternalServerError,
            Title = "SOME_KIND_OF_ERROR_OCCURRED_IN_THE_API"
        };

        _options.AddResponseDetails?.Invoke(context, exception, error);

        var innerExMessage = GetInnermostExceptionMessage(exception);

        var level = _options.DetermineLogLevel?.Invoke(exception) ?? LogLevel.Error;

        _logger.Log(level, exception, "BADNESS!!! " + innerExMessage + " -- {ErrorId}.", error.Id);

        var result = _serializer.Serialize(error);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return context.Response.WriteAsync(result);
    }

    private string GetInnermostExceptionMessage(Exception exception) =>
             exception.InnerException.IsNotNull()
            ?
            GetInnermostExceptionMessage(exception.InnerException)
            :
            exception.Message;
}

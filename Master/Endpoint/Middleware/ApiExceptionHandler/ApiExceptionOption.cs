namespace Master.Endpoint.Middleware.ApiExceptionHandler;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class ApiExceptionOption
{
    public Action<HttpContext, Exception, ApiError> AddResponseDetails { get; set; }
    public Func<Exception, LogLevel> DetermineLogLevel { get; set; }
}
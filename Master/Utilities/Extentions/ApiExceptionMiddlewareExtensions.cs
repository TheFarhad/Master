namespace Master.Utilities.Extentions;

using Microsoft.AspNetCore.Builder;
using Endpoint.Middleware.ApiExceptionHandler;

public static class ApiExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder)
    {
        var option = new ApiExceptionOption();
        return builder.UseMiddleware<ApiExceptionMiddleware>(option);
    }

    public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder,
        Action<ApiExceptionOption> configureOption)
    {
        var option = new ApiExceptionOption();
        configureOption(option);

        return builder.UseMiddleware<ApiExceptionMiddleware>(option);
    }
}

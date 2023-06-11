namespace Master.Utilities.Extentions;

using Master.Core.Contract.Application.Command;
using Master.Core.Contract.Application.Event;
using Master.Core.Contract.Application.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public static class HttpContextExtentions
{
    public static string GetClaim(this HttpContext source, string claimType) =>
         source?
        .User?
        .Claims
        .FirstOrDefault(_ => _.Type == claimType)?
        .Value ?? String.Empty;

    public static IEventDispatcher EventDispatcher(this HttpContext source) =>
       source.Service<IEventDispatcher>();

    public static ICommandDispatcher CommandDispatcher(this HttpContext source) =>
         source.Service<ICommandDispatcher>();

    public static IQueryDispatcher QueryDispatcher(this HttpContext source) =>
         source.Service<IQueryDispatcher>();


    public static T Service<T>(this HttpContext source) => source.RequestServices.GetService<T>();
}

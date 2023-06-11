namespace Master.Endpoint.Controller;

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Utilities.Extentions;
using Core.Contract.Application.Event;
using Core.Contract.Application.Query;
using Core.Contract.Application.Common;
using Core.Contract.Application.Command;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : Controller
{
    protected IEventDispatcher EventDispatcher => HttpContext.EventDispatcher();
    protected ICommandDispatcher CommandDispatcher => HttpContext.CommandDispatcher();
    protected IQueryDispatcher QueryDispatcher => HttpContext.QueryDispatcher();

    protected async Task<IActionResult> AddAsync<TCommand>(TCommand source) where TCommand : ICommand
    {
        var response = await CommandDispatcher.DispatchAsync<TCommand>(source);

        return
            response.Status == ServiceStatus.Ok
            ?
            StatusCode((int)HttpStatusCode.Created)
            :
             BadRequest(response.Errors);
    }

    protected async Task<IActionResult> AddAsync<TCommand, TPayload>(TCommand source) where TCommand : ICommand<TPayload>
    {
        var response = await CommandDispatcher.DispatchAsync<TCommand, TPayload>(source);

        return
            response.Status == ServiceStatus.Ok
            ?
            StatusCode((int)HttpStatusCode.Created, response.Payload)
            :
             BadRequest(response.Errors);
    }

    protected async Task<IActionResult> EditAsync<TCommand>(TCommand source) where TCommand : ICommand
    {
        var response = await CommandDispatcher.DispatchAsync<TCommand>(source);

        return
         response.Status == ServiceStatus.Ok
         ?
         StatusCode((int)HttpStatusCode.OK)
         :
         response.Status == ServiceStatus.NotFound
         ?
          StatusCode((int)HttpStatusCode.NotFound, source)
          :
          BadRequest(response.Errors);
    }

    protected async Task<IActionResult> EditAsync<TCommand, TPayload>(TCommand source) where TCommand : ICommand<TPayload>
    {
        var response = await CommandDispatcher.DispatchAsync<TCommand, TPayload>(source);

        return
       response.Status == ServiceStatus.Ok
       ?
       StatusCode((int)HttpStatusCode.OK, response.Payload)
       :
       response.Status == ServiceStatus.NotFound
       ?
        StatusCode((int)HttpStatusCode.NotFound, source)
        :
        BadRequest(response.Errors);
    }

    protected async Task<IActionResult> RemoveAsync<TCommand>(TCommand source) where TCommand : ICommand
    {
        var response = await CommandDispatcher.DispatchAsync<TCommand>(source);

        return
         response.Status == ServiceStatus.Ok
         ?
         StatusCode((int)HttpStatusCode.OK)
         :
         response.Status == ServiceStatus.NotFound
         ?
          StatusCode((int)HttpStatusCode.NotFound, source)
          :
          BadRequest(response.Errors);
    }

    protected async Task<IActionResult> RemoveAsync<TCommand, TPayload>(TCommand source) where TCommand : ICommand<TPayload>
    {
        var response = await CommandDispatcher.DispatchAsync<TCommand, TPayload>(source);

        return
         response.Status == ServiceStatus.Ok
         ?
         StatusCode((int)HttpStatusCode.OK, response.Payload)
         :
         response.Status == ServiceStatus.NotFound
         ?
          StatusCode((int)HttpStatusCode.NotFound, source)
          :
          BadRequest(response.Errors);
    }

    protected async Task<IActionResult> GetAsync<TQuery, TPayload>(TQuery source) where TQuery : IQuery<TPayload>
    {
        var response = await QueryDispatcher.DispatchAsync<TQuery, TPayload>(source);

        return
            response.Status == ServiceStatus.Ok
            ?
            StatusCode((int)HttpStatusCode.OK, response.Payload)
            :
            response.Status == ServiceStatus.NotFound || response.Payload == null
            ?
            StatusCode((int)HttpStatusCode.NoContent, source)
            :
            response.Status == ServiceStatus.InvalidDomainState || response.Status == ServiceStatus.ValidationError
            ?
            BadRequest(response.Errors)
            :
            BadRequest(response.Errors);
    }
}

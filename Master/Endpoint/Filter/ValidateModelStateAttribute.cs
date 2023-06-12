namespace Master.Endpoint.Filter;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidateModelStateAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var modelState = context.ModelState;
        if (!modelState.IsValid)
        {
            var error = modelState
                .Where(_ => _.Value.Errors.Any())
                .Select(_ => String.Join(",", _.Value.Errors.Select(e => e.ErrorMessage)))
                .ToList();

            context.Result = new BadRequestObjectResult(error);
        }
    }
}

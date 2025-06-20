using Microsoft.AspNetCore.Mvc.Filters;
using RaspberryPi.API.Services;

namespace RaspberryPi.API.Filters;

public class RequestCounterFilter : IActionFilter
{
    private readonly RequestCounterService _counter;

    public RequestCounterFilter(RequestCounterService counter)
    {
        _counter = counter;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var controller = context.ActionDescriptor.RouteValues["controller"] ?? "Unknown";
        var action = context.ActionDescriptor.RouteValues["action"] ?? "Unknown";
        _counter.Increment(controller, action);
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}

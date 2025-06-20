using Microsoft.AspNetCore.Mvc.Filters;
using RaspberryPi.API.Extensions;
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
        var ipAddress = context.HttpContext.GetClientIpAddress();
        _counter.Increment(controller, action, ipAddress);
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
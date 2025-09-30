using RaspberryPi.Domain.Extensions;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Emails;

namespace RaspberryPi.API.Middlewares;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IEmailInfraService _service;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IEmailInfraService service)
    {
        _next = next;
        _logger = logger;
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await TrySendEmailAsync(context, ex);
            throw;
        }
    }

    private async Task TrySendEmailAsync(HttpContext context, Exception ex)
    {
        try
        {
            var email = new Email
            {
                To = "bruno.campiol@gmail.com",
                Subject = "Unhandled Exception Occurred",
                Body = $@"
                    Exception: {ex.GetType().Name}
                    Message(s): {ex.AllMessages()}
                    Path: {context.Request.Path}
                    TraceId: {context.TraceIdentifier}
                    StackTrace: {ex.StackTrace}
                ",
                IsBodyHtml = false
            };

            await _service.SendEmailAsync(email);
        }
        catch (Exception emailEx)
        {
            _logger.LogError(emailEx, "Failed to send error notification email.");
        }
    }
}
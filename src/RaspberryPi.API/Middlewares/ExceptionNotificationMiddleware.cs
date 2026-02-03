using RaspberryPi.Domain.Extensions;
using RaspberryPi.Infrastructure.Interfaces;
using RaspberryPi.Infrastructure.Models.Emails;

namespace RaspberryPi.API.Middlewares;

public sealed class ExceptionNotificationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;
    private readonly IEmailInfraService _service;
    private readonly ILogger<ExceptionNotificationMiddleware> _logger;

    public ExceptionNotificationMiddleware(RequestDelegate next,
        ILogger<ExceptionNotificationMiddleware> logger,
        IHostEnvironment env,
        IEmailInfraService service)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _env = env ?? throw new ArgumentNullException(nameof(env));
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
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var body = BuildBody(context, ex, _env.EnvironmentName);

            var email = new Email
            {
                To = "bruno.campiol@gmail.com",
                Subject = "Unhandled Exception Occurred",
                Body = body,
                IsBodyHtml = false
            };

            await _service.SendEmailAsync(email);
        }
        catch (Exception sendException)
        {
            _logger.LogError(sendException, "Failed to send error notification email.");
        }
    }

    private static string BuildBody(HttpContext context, Exception ex, string envName)
    {
        // Use ToString() for full details, but truncate to avoid gigantic emails.
        var details = ex.ToString();
        if (details.Length > 20_000) details = details[..20_000] + "\n\n[TRUNCATED]";

        return $"""
                Environment: {envName}
                ExceptionType: {ex.GetType().FullName}
                Message(s): {ex.AllMessages()}
                Path: {context.Request.Path}
                Method: {context.Request.Method}
                TraceId: {context.TraceIdentifier}

                Details:
                {details}
                """;
    }
}
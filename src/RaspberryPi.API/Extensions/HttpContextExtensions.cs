using System.Net;

namespace RaspberryPi.API.Extensions;

public static class HttpContextExtensions
{
    /// <summary>
    /// Gets the real client IP address, taking into account X-Forwarded-For header if present
    /// </summary>
    /// <param name="context">The HttpContext</param>
    /// <returns>The client's IP address</returns>
    public static string GetClientIpAddress(this HttpContext context)
    {
        var clientIp = context.Connection.RemoteIpAddress?.ToString();
        string? forwardedHeader = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedHeader))
        {
            clientIp = forwardedHeader
                .Split([','], StringSplitOptions.RemoveEmptyEntries)
                .Select(ip => ip.Trim())
                .FirstOrDefault(ip => !IPAddress.IsLoopback(IPAddress.Parse(ip)));
        }

        return clientIp ?? string.Empty;
    }
}
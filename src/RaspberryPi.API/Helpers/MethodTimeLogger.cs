using System.Reflection;

namespace RaspberryPi.API.Helpers;

public static class MethodTimeLogger
{
    private static ILogger? _logger;

    public static ILogger Logger
    {
        get => _logger ?? throw new InvalidOperationException("Logger has not been initialized.");
        set => _logger = value ?? throw new ArgumentNullException(nameof(value));
    }

    public static void Log(MethodBase methodBase, TimeSpan timeSpan)
    {
        Logger.LogInformation("{Class}.{Method} {Duration}",
            methodBase.DeclaringType!.Name,
            methodBase.Name,
            timeSpan);
    }
}
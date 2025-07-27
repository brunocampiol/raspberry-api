using System.Reflection;

namespace RaspberryPi.API.Helpers;

public static class MethodTimeLogger
{
    public static ILogger Logger;

    public static void Log(MethodBase methodBase, TimeSpan timeSpan)
    {
        Logger.LogInformation("{Class}.{Method} {Duration}",
            methodBase.DeclaringType!.Name,
            methodBase.Name,
            timeSpan);
    }
}
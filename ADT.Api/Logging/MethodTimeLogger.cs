using System.Reflection;

namespace ADT.Api.Logging;

public static class MethodTimeLogger
{
    public static ILogger Logger;
    
    public static void Log(MethodBase methodBase, TimeSpan timeSpan, string message)
    {
        Logger.LogTrace("{ClassName}.{Method} {Duration}", methodBase.DeclaringType.Name, methodBase.Name, timeSpan);
    }
}
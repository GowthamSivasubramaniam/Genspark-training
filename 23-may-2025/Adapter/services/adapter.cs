using Adapter.Interfaces;
namespace Adapter.services
{
public class LegacyLoggerAdapter : ILogger
{
    private readonly LegacyLogger _legacyLogger;

    public LegacyLoggerAdapter(LegacyLogger legacyLogger)
    {
        _legacyLogger = legacyLogger;
    }

    public void Log(string message)
    {
        _legacyLogger.WriteToLog(message);
    }
}
}

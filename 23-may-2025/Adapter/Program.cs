using Adapter.Interfaces;
using Adapter.services;

class Program
{
    static void Main(string[] args)
    {
        ILogger logger = new LegacyLoggerAdapter(new LegacyLogger());
        logger.Log("This is an adapted log message.");
    }
}

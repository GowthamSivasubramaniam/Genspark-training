using Factory.Interface;
namespace Factory.Services
{

public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine("[Log] " + message);
        }
    }
}
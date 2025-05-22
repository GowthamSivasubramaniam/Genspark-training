namespace CalculatorApp.Services
{
    public class ConsoleLogger : Interfaces.ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine("[Log] " + message);
        }
    }
}

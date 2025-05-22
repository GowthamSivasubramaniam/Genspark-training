


namespace CalculatorApp.Services
{
public class FileLogger : Interfaces.ILogger
{
    private readonly string _filePath;

    public FileLogger(string filePath = "log.txt")
    {
        _filePath = filePath;
    }

    public void Log(string message)
    {
        File.AppendAllText(_filePath, $"[FileLog] {DateTime.Now}: {message}{Environment.NewLine}");
        Console.WriteLine($"Result written to {_filePath}");
    }
}
}
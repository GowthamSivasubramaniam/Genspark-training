public class FileWriter
{

    private static FileWriter _instance;
    private static readonly object _lock = new object();
    private FileWriter() { }
    public static FileWriter GetInstance()
    {
       if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new FileWriter();
                    Console.WriteLine("New instance created");
                }
            }
        }
        else
        {
            Console.WriteLine("Existing instance returned");
        }
        return _instance;
    }



    public void Write(string message)
    {
        string filePath = "log.txt";
        File.AppendAllText(filePath, message + Environment.NewLine);
        Console.WriteLine($"Result written to {filePath}");
    }
}
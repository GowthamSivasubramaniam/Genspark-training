

using Factory.Interface;
using Factory.Services;
public class Program
{
    static void Main(string[] args)
    {
        int choice;
        Console.WriteLine("Choose Logger Type:");
        Console.WriteLine("1. Console Logger");
        Console.WriteLine("2. File Logger");
        while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2))
        {
            Console.WriteLine("Invalid choice. Please enter 1 or 2.");
        }
        ILogger logger ;

        if (choice == 1)
        {
            logger = new ConsoleLogger();
        }
        else
        {
            logger = new FileLogger();
        }
        Console.WriteLine("Enter message to log:");
        string message = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(message))
        {
            Console.WriteLine("Invalid input. Please enter a valid message:");
            message = Console.ReadLine();
        }
        logger.Log(message);
        Console.WriteLine("Message logged successfully.");
    }
}
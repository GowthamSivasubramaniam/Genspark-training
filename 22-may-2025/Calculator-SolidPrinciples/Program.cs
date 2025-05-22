using CalculatorApp.Interfaces;
using CalculatorApp.Services;

class Program
{
    static void Main()
    {

               
                Console.WriteLine("Welcome to the Calculator App!");
                while (true)
                {
                Console.WriteLine("Choose calculator: 1. Basic  2. Scientific  3. Exit");
                int calcChoice;
                while (!int.TryParse(Console.ReadLine(), out calcChoice) || (calcChoice < 1 || calcChoice > 3))
                {
                    Console.WriteLine("Invalid input. Enter 1 for Basic, 2 for Scientific, or 3 to Exit:");
                }
                Console.WriteLine("Choose logger: 1. Console  2. File");
                int logChoice;
                while (!int.TryParse(Console.ReadLine(), out logChoice) || (logChoice != 1 && logChoice != 2))
                {
                    Console.Write("Invalid choice. Choose logger: 1. Console  2. File: ");
                }

              

                if(calcChoice == 3)
                {
                    Console.WriteLine("Exiting...");
                    return;
                }

                
                ILogger logger = (logChoice == 1) ? new ConsoleLogger() : new FileLogger();
                ICalculator calculator = (calcChoice == 1)
                    ? new BasicCalculator(logger)
                    : new ScientificCalculator(logger);

                calculator.ShowMenu();

                int option;
                Console.Write("Choose operation: ");
                while (!int.TryParse(Console.ReadLine(), out option))
                {
                    Console.Write("Invalid input. Enter a valid operation number: ");
                }

                Console.Write("Enter value A: ");
                double a;
                while (!double.TryParse(Console.ReadLine(), out a))
                {
                    Console.Write("Invalid input. Enter a valid number for A: ");
                }

                double b = 0;
                if (option <= 4) 
                {
                    Console.Write("Enter value B: ");
                    while (!double.TryParse(Console.ReadLine(), out b))
                    {
                        Console.Write("Invalid input. Enter a valid number for B: ");
                    }
                }

                try
                {
                    calculator.Calculate(option, a, b);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                
                }
    }
}

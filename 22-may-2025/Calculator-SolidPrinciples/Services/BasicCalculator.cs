using CalculatorApp.Interfaces;

namespace CalculatorApp.Services
{
    public class BasicCalculator : ICalculator, IBasicOperations
    {
        private readonly ILogger _logger;

        public BasicCalculator(ILogger logger)
        {
            _logger = logger;
        }

        public void ShowMenu()
        {
            Console.WriteLine("1. Add\n2. Subtract\n3. Multiply\n4. Divide");
        }

        public void Calculate(int option, double a, double b = 0)
        {
            string operation = option switch
            {
                1 => "Add",
                2 => "Subtract",
                3 => "Multiply",
                4 => "Divide",
                _ => throw new ArgumentException("Invalid Option")
            };
            double result = option switch
            {
                1 => Add(a, b),
                2 => Subtract(a, b),
                3 => Multiply(a, b),
                4 => Divide(a, b),
                _ => throw new ArgumentException("Invalid Option")
            };
            _logger.Log($"Basic Calculator: {operation} , values: {a}, {b} , Result: {result}");
           
        }

        public double Add(double a, double b) => a + b;
        public double Subtract(double a, double b) => a - b;
        public double Multiply(double a, double b) => a * b;
        public double Divide(double a, double b) => b == 0 ? throw new DivideByZeroException() : a / b;
    }
}

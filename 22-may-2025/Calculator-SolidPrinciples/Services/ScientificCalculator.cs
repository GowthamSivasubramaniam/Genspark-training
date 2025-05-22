using CalculatorApp.Interfaces;

namespace CalculatorApp.Services
{
    public class ScientificCalculator : ICalculator, IBasicOperations, IScientificOperations
    {
        private readonly ILogger _logger;

        public ScientificCalculator(ILogger logger)
        {
            _logger = logger;
        }

        public void ShowMenu()
        {
            Console.WriteLine("1. Add\n2. Subtract\n3. Multiply\n4. Divide\n5. Sine\n6. Cosine");
        }

        public void Calculate(int option, double a, double b = 0)
        {
            string operation = option switch
            {
                1 => "Add",
                2 => "Subtract",
                3 => "Multiply",
                4 => "Divide",
                5 => "Sine",
                6 => "Cosine",
                _ => throw new ArgumentException("Invalid Option")
            };
            double result = option switch
            {
                1 => Add(a, b),
                2 => Subtract(a, b),
                3 => Multiply(a, b),
                4 => Divide(a, b),
                5 => Sine(a),
                6 => Cosine(a),
                _ => throw new ArgumentException("Invalid Option")
            };
            _logger.Log($"Scientific Calculator: {operation} , values{a},{b} , Result: {result}");
           
        }

        public double Add(double a, double b) => a + b;
        public double Subtract(double a, double b) => a - b;
        public double Multiply(double a, double b) => a * b;
        public double Divide(double a, double b) => b == 0 ? throw new DivideByZeroException() : a / b;
        public double Sine(double a) => Math.Sin(a);
        public double Cosine(double a) => Math.Cos(a);
    }
}

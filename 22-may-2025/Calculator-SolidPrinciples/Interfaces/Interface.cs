namespace CalculatorApp.Interfaces
{
    public interface IBasicOperations
    {
        double Add(double a, double b);
        double Subtract(double a, double b);
        double Multiply(double a, double b);
        double Divide(double a, double b);
    }

    public interface IScientificOperations
    {
        double Sine(double a);
        double Cosine(double a);
    }

    public interface ILogger
    {
        void Log(string message);
    }

    public interface ICalculator
    {
        void ShowMenu();
        void Calculate(int option, double a, double b = 0);
    }
}

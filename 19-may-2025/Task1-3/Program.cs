
class Program
{
    static void greet(string? name)
    {
        name = name.Trim();
        if (name.Length == 0)
        {
            Console.WriteLine("Enter a valid name ");
        }
        else
        {
            Console.WriteLine("Greetings " + name + "!");
        }
    }
    static void printLargest(int a, int b)
    {
        if (a > b)
        {
            Console.WriteLine($"The largest number is {a}");
        }
        else if (a == b)
        {
            Console.WriteLine($"Both numbers are equal");
        }

        else
        {
            Console.WriteLine($"The largest number is {b}");
        }
    }

    static void PerformAddoperation(int a, int b)
    {
        Console.WriteLine($"Addition : {a + b}");
    }
    static void PerformSuboperation(int a, int b)
    {
        Console.WriteLine($"Subtraction: {a - b}");
    }
    static void PerformMultiplyOperation(int a, int b)
    {
        Console.WriteLine($"Multiplication: {a * b}");
    }
    static void PerformDivideOperation(int a, int b)
    {
        if (b > 0)
            Console.WriteLine($"Division: {(a / b):F2}");
        else
            Console.WriteLine($"Division by zero is Not possible");
    }



    static void Main()
    {
        //q1
        System.Console.WriteLine("Enter your name");
        string? n = Console.ReadLine();
        Console.WriteLine("Enter Two Numbers");
        greet(n);
        //q2
        int num1 = Convert.ToInt32(Console.ReadLine());
        int num2 = Convert.ToInt32(Console.ReadLine());
        printLargest(num1, num2);
        //q3
        Console.WriteLine("Choose the options from below");
        Console.WriteLine("1. Add \n 2. Subtract \n 3. Multiply \n 4. Divide \n Enter Your Choice");
        int choice = Convert.ToInt32(Console.ReadLine()); ;
        switch (choice)
        {
            case 1:
                PerformAddoperation(num1, num2);
                break;
            case 2:
                PerformSuboperation(num1, num2);
                break;
            case 3:
                PerformMultiplyOperation(num1, num2);
                break;
            case 4:
                PerformDivideOperation(num1, num2);
                break;
            default:
                Console.WriteLine("Enter a Valid choice");
                break;

        }
    }
}
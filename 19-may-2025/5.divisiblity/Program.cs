
class Program
{

    static void CheckDivisibleby7(List<int> numbers)
    {

        foreach (int number in numbers)
        {
            if (number % 7 == 0)
                c++;
        }
        Console.WriteLine($"The number of elements divisible by 7 is {c}");
    }

    static void Main()
    {
        System.Console.WriteLine("please Enter numbers seperated by space and once done press enter");
       string? nums= System.Console.ReadLine();

        List<int> numbers = new List<int>();
       if (!string.IsNullOrWhiteSpace(nums))
        {
            foreach (string s in nums.Split(' '))
            {
                if (int.TryParse(s, out int num))
                {
                    numbers.Add(num);
                }
                else
                {
                    Console.WriteLine($"'{s}' is not a valid number and will be skipped.");
                }
            }
        }
    }
}
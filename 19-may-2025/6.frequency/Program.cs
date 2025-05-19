
class Program
{
    static void  freq_Calculator(List<int> numbers)
    {
        Dictionary<int, int> frequency = new Dictionary<int, int>();

        foreach (int number in numbers)
        {
            if (frequency.ContainsKey(number))
                frequency[number]++;
            else
                frequency[number] = 1;
        }

      
        foreach (var pair in frequency)
        {
            Console.WriteLine($"Number {pair.Key} occurs {pair.Value} times.");
        }
    }
    static void Main(string[] args)
    {
        System.Console.WriteLine("please Enter numbers seperated by space and once done press enter");
        string? nums = System.Console.ReadLine();

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


            freq_Calculator(numbers);
        }
        else
        {
            Console.WriteLine("No numbers were entered.");
        }

    }
}
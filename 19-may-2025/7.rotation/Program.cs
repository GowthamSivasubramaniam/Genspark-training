class Program
{
    static void Rotatearray(List<int> nums)
    {
        int[] arr = nums.ToArray();
        int first = arr[0];
        for (int i = 0; i < arr.Length - 1; i++)
        {
            arr[i] = arr[i + 1];
        }
        arr[arr.Length - 1] = first;
        
        Console.WriteLine("Array elements after rotatation: " + string.Join(", ", arr));
        
    }
    public static void Main(string[] args)
    {
        string? numbers = Console.ReadLine();
        List<int> Arr = new List<int>();
        if (!string.IsNullOrEmpty(numbers))
        {
            foreach (string s in numbers.Split(' '))
            {
                if (int.TryParse(s, out int number))
                {
                    Arr.Add(number);
                }
                else
                {
                    System.Console.WriteLine($"The number {number} is invalid and will be skipped");
                }
            }
            Rotatearray(Arr);
        }
        else
        {
            System.Console.WriteLine("No numbers were entered.");
        }
    }
}

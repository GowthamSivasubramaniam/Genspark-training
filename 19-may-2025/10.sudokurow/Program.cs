class Program
{

    static void Main(string[] args)
    {
        Console.WriteLine("Enter Sudoku row elements (space-separated):");
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
                    System.Console.WriteLine($"The number {s} is invalid and will be skipped");
                }
            }
            int[] arr = Arr.ToArray();
            int n = arr.Length;
            Checkrowisvalidsudoku(arr, n);
        }
        else
        {
            System.Console.WriteLine("No numbers were entered.");
        }


    }

    private static void Checkrowisvalidsudoku(int[] arr, int n)
    {
        bool flag = true;
        HashSet<int> set = new HashSet<int>();
        if (n != 9)
        {
            Console.WriteLine("Invalid Sudoku , row length is not equal to 9");
            flag = false;
            return;
        }
        for (int i = 0; i < n; i++)
        {
            if (arr[i] < 1 || arr[i] > 9)
            {
                Console.WriteLine("Invalid Sudoku , value out of range");
                flag = false;
                break;

            }
            if (set.Contains(arr[i]))
            {
                Console.WriteLine($"Invalid Sudoku , set has duplicate value {arr[i]}");
                flag = false;
                break;

            }
            set.Add(arr[i]);
        }

        if (flag)
        {
            Console.WriteLine("Valid Sudoku row.");
        }


    }
}
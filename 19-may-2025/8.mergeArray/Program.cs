class Program
{
    static void MergeArray(List<int> Arr1,List<int> Arr2)
    {
        int[] arr1 = Arr1.ToArray();
        int[] arr2 = Arr2.ToArray();
        int [] mergedarray = new int[arr1.Length + arr2.Length];

        for (int i = 0; i < arr1.Length; i++)
        {
            mergedarray[i]=arr1[i];
        }
        for (int i = 0; i < arr2.Length; i++)
        {
            mergedarray[i+arr1.Length]=arr2[i];
        }
       
        
        Console.WriteLine("Array elements after Merging: " + string.Join(", ", mergedarray));
        
    }
    public static void Main(string[] args)
    {
        System.Console.WriteLine("Enter Array 1 elements");
        string? arr1 = Console.ReadLine();
        System.Console.WriteLine("Enter Array 2 elements");
        string? arr2 = Console.ReadLine();
        List<int> Arr1 = new List<int>();
        List<int> Arr2 = new List<int>();
        if (!string.IsNullOrEmpty(arr1))
        {
            foreach(string s in arr1.Split(' '))
            {
                if (int.TryParse(s, out int number))
                {
                    Arr1.Add(number);
                }
                else
                {
                    System.Console.WriteLine($"The number {number} is invalid and will be skipped");
                }
            }
        }
        else
        {
            System.Console.WriteLine("No numbers were entered.");
        }
        if (!string.IsNullOrEmpty(arr2))
        {
            foreach (string s in arr2.Split(' '))
            {
                if (int.TryParse(s, out int number))
                {
                    Arr2.Add(number);
                }
                else
                {
                    System.Console.WriteLine($"The number {number} is invalid and will be skipped");
                }
            }
        }
        else
        {
            System.Console.WriteLine("No numbers were entered.");
        }
        if (Arr1.Count == 0 && Arr2.Count == 0)
        {
            System.Console.WriteLine("both of the arrays are empty, cannot merge.");
        }
        else
        {
            MergeArray(Arr1, Arr2);
        }
    }
}
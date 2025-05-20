using System;

class Posts
{
    public string Description { get; set; }
    public int Likes { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter number of users:");
        int n = Convert.ToInt32(Console.ReadLine());

        Posts[][] array = new Posts[n][];

        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"Enter number of posts for User {i + 1}:");
            int postCount = Convert.ToInt32(Console.ReadLine());

            array[i] = new Posts[postCount];

            for (int j = 0; j < postCount; j++)
            {
                array[i][j] = new Posts();

                Console.Write($"Enter description for Post {j + 1}: ");
                array[i][j].Description = Console.ReadLine();

                Console.Write($"Enter likes for Post {j + 1}: ");
                array[i][j].Likes = Convert.ToInt32(Console.ReadLine());
            }
        }

       
        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"User {i + 1} has {array[i].Length} post(s):");
            for (int j = 0; j < array[i].Length; j++)
            {
                Console.WriteLine($"  Description : {array[i][j].Description} , Likes : {array[i][j].Likes}");
            }
        }
    }
}

using Flyweight.Models;
using Flyweight.Interfaces;

class Program
{
    static void Main(string[] args)
    {
        var treeFactory = new TreeFactory();

        Random random = new();

        for (int i = 0; i < 10; i++)
        {
            string type = i % 2 == 0 ? "Oak" : "Pine";
            int x = random.Next(0, 100);
            int y = random.Next(0, 100);

            ITree tree = treeFactory.GetTree(type);
            tree.Display(x, y);
        }

        Console.WriteLine("Only two tree objects were created and reused!");
    }
}

using proxy.Interfaces;
using proxy.Services;
using proxy.Models;


class Program
{
    static void Main(string[] args)
    {
       
    
        int choice;
        string role = string.Empty;
        string filePath = "log.txt"; 
        Console.WriteLine("Please Enter username:");
        string username = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Invalid input. Please enter a valid username:");
            username = Console.ReadLine();
        }
        Console.WriteLine("Choose Role:");
        Console.WriteLine("1. Admin"); 
        Console.WriteLine("2. User");
        Console.WriteLine("3. Guest");
        while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2 && choice != 3))
        {
            Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
        }
        switch (choice)
        {
            case 1:
                role = "admin";
                break;
            case 2:
                role = "user";
                break;
            case 3:
                role = "guest";
                break;
        }
        User user = new User { Name = username, role = role };
        IFile proxyFile = new ProxyFile(filePath, user);
        string content = proxyFile.Read();

        Console.WriteLine("File Content: " + content);
    }
}
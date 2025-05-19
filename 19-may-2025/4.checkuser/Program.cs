
class Program
{

    static void CheckUser()
    {
        int c = 0;
        while (true)
        {
            Console.Write("Enter UserName: ");
            string? name = Console.ReadLine();
            Console.Write("Enter Password: ");
            string? password = Console.ReadLine();
            if (name.Equals("Admin") && password.Equals("pass"))
            {
                Console.WriteLine("Successfully loggedin");
                break;
            }
            else if (c < 2)
            {
                c += 1;
                Console.WriteLine($"Invalid Credentials only {3 - c} attempts more");
            }
            else
            {
                Console.WriteLine($"Invalid Credentials .Exiting ...");
                break;
            }
        }

    }
    

    static void Main()
    {
       
        CheckUser();

    }
}

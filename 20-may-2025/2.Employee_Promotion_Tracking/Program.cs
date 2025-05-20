
public class Employee
{
    public string? Employee_name { get; set; }
}

public class Program
{
    private static List<Employee> employees = new List<Employee>();
    


    // 1. Input employees in order of Promotion
    private static void InsertEmployees()
    {
        System.Console.WriteLine("Enter names of employee space seperated");
        string? names = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(names))
        {
            Console.WriteLine("Invalid, please re-enter the names:");
            names = Console.ReadLine();
        }

        foreach (var item in names.Split(' '))
        {
            string name = item;
            while (string.IsNullOrWhiteSpace(name) || !name.All(char.IsLetter))
            {
                Console.WriteLine($"Please re-enter a valid name instead of {name}");
                name = Console.ReadLine();
            }
            employees.Add(new Employee { Employee_name = name });
        }
        System.Console.WriteLine("successfully added the employees");

        Console.WriteLine($" Allocated memeory space for Employees list before trimming is {employees.Capacity}");
        employees.TrimExcess();
        Console.WriteLine($" Allocated memeory space for Employees list after trimming is {employees.Capacity}");
    }


    // 2. Get the employee by promotion order
    private static void DisplayEmployeeBYorder(int ordnumber)
    {

        if (ordnumber > employees.Count || ordnumber < 1)
        {
            System.Console.WriteLine("No employee found in this order");
            return;
        }
        System.Console.WriteLine($"The employee in {ordnumber} order is {employees[ordnumber - 1].Employee_name}");
    }

    // 3. Display all the employees in order of promotion
    private static void DisplayAllEmployeesinOrder()
    {
        System.Console.WriteLine("The employees in order of promotion are");
        foreach (var employee in employees)
        {
            System.Console.WriteLine(employee.Employee_name);
        }
    }
    
    // 4. Display all the employees in sorted order
    private static void DisplayAllEmployees()
    {
        System.Console.WriteLine("The employees are");
        var sortedEmployees = employees.OrderBy(e => e.Employee_name).ToList();
        foreach (var employee in sortedEmployees)
        {
            System.Console.WriteLine(employee.Employee_name);
        }
    }


    public static void Main()
    {


        while (true)
        {
            System.Console.WriteLine("\n\nChoices \n 1.Input employees in order of Promotion \n 2.Get the employee by promotion order\n 3.Display all the employees in order of promotion\n 4.Display all the employees \n 5.Exit");
            System.Console.WriteLine("Please enter Your choice");
            int choice;
            int.TryParse(Console.ReadLine(), out choice);
            switch (choice)
            {
                case 1:
                    InsertEmployees();
                    break;


                case 2:
                    System.Console.WriteLine("Please enter the promotion order number");
                    int ordnumber = 0;
                    while (!int.TryParse(Console.ReadLine(), out ordnumber))
                    {
                        System.Console.WriteLine("Invalid promotion order number");
                    }
                    DisplayEmployeeBYorder(ordnumber);
                    break;


                case 3:
                    DisplayAllEmployeesinOrder();
                    break;



                case 4:
                    DisplayAllEmployees();
                    break;

                case 5:
                    System.Console.WriteLine("Exiting the program");
                    return;
                default:
                    System.Console.WriteLine("Invalid choice");
                    break;

            }
             
        }
       

        
    }
}

public class Employee
{
    public string? Employee_name { get; set; }
    public double salary { get; set; }
    public int age { get; set; }
}

public class Program
{
    public static Dictionary<int, Employee> employees = new Dictionary<int, Employee>();


    // 1. Insert employee details
    private static void InsertEmployees()
    {
        System.Console.WriteLine("Please Enter employee ID");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            System.Console.WriteLine("Invalid ID");
        }
        if (employees.ContainsKey(id))
        {
            System.Console.WriteLine("Employee ID already exists");
            return;
        }
        System.Console.WriteLine("Please Enter Employee Name");
        string? name = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(name) || !name.All(char.IsLetter))
        {
            Console.WriteLine($"Please re-enter a valid name instead of {name}");
            name = Console.ReadLine();
        }

        double Salary;
        System.Console.WriteLine("Please Enter Salary");
        while (!double.TryParse(Console.ReadLine(), out Salary))
        {
            System.Console.WriteLine("Invalid salary , Re-enter the salary please");
        }
        int Age;
        System.Console.WriteLine("Please Enter Age");
        while (!int.TryParse(Console.ReadLine(), out Age))
        {
            System.Console.WriteLine("Invalid age, Re-enter the age please");
        }

        Employee e = new Employee { Employee_name = name, salary = Salary, age = Age };
        employees[id] = e;
        Console.WriteLine("Employee inserted successfully ");
    }
    



    // 2. Display employee details by ID

    private static void DisplayEmployeeByID(int id)
    {

        var employee = employees.Where(e => e.Key == id).Select(e => e.Value).FirstOrDefault();

        if (employee == null)
        {
            Console.WriteLine("Employee Not found");
            return;
        }

        Console.WriteLine($"Name   : {employee.Employee_name}");
        Console.WriteLine($"Salary : {employee.salary}");
        Console.WriteLine($"Age    : {employee.age}");


    }
     
    // 3 . Display all employees in order of salary
    private static void SortEmployeesbySalary()
    {
        if (employees.Count == 0)
        {
            System.Console.WriteLine("No employees found");
            return;
        }
        System.Console.WriteLine("The employees in order of salary are");
        var sortedEmployees = employees.OrderBy(e => e.Value.salary).ToList();
        foreach (var employee in sortedEmployees)
        {

            System.Console.WriteLine($"Id : {employee.Key}");
            System.Console.WriteLine($"Name : {employee.Value.Employee_name}");
            System.Console.WriteLine($"Salary : {employee.Value.salary}");
            System.Console.WriteLine($"Age : {employee.Value.age} \n ------------------------------------");
        }
    }
    
    
    // 4. Display all employees with given name

    private static void DisplayAllEmployeesWithGivenName(string name)
    {
        var Employees = employees.Where(e => e.Value.Employee_name == name).ToList();
        if (Employees.Count == 0)
        {
            System.Console.WriteLine("No employees found with the given name");
            return;
        }
        System.Console.WriteLine("The employees with the given name are");
        foreach (var employee in Employees)
        {
            System.Console.WriteLine($"Name : {employee.Value.Employee_name}");
            System.Console.WriteLine($"Salary : {employee.Value.salary}");
            System.Console.WriteLine($"Age : {employee.Value.age}");
        }

    }

    // 5. Display all employees elder than given employee
    private static void DisplayAllEmployeesAboveGivenAge(int id)
    {
        var employeeage = employees
        .Where(e => e.Key == id)
        .Select(e => e.Value.age)
        .FirstOrDefault();
        if (employeeage == 0)
        {
            System.Console.WriteLine("Employee Not found");
            return;
        }
        var Employees = employees.Where(e => e.Value.age > employeeage).ToList();
        if (Employees.Count == 0)
        {
            System.Console.WriteLine("No employees has age above the given age");
            return;
        }
        System.Console.WriteLine("The employees has age above the given age");
        foreach (var employee in Employees)
        {
            System.Console.WriteLine($"Name : {employee.Value.Employee_name}");
            System.Console.WriteLine($"Salary : {employee.Value.salary}");
            System.Console.WriteLine($"Age : {employee.Value.age}");
        }

    }

    // 6. Modify employee details by ID

    private static void ModifyEmployeeByID(int id)
    {

        if (!employees.ContainsKey(id))
        {
            System.Console.WriteLine("Employee Not found");
            return;
        }
        
        System.Console.WriteLine($"Name : {employees[id].Employee_name}");
        System.Console.WriteLine($"Salary : {employees[id].salary}");
        System.Console.WriteLine($"Age : {employees[id].age}");

        System.Console.WriteLine("\n choose the field to modify");
        int choice;
        System.Console.WriteLine("1.Name \n 2.Salary \n 3.Age");
        while (!int.TryParse(Console.ReadLine(), out choice))
        {
            System.Console.WriteLine("Invalid choice");
        }
        switch (choice)
        {
            case 1:
                System.Console.WriteLine("Please Enter Employee Name");
                string? name = Console.ReadLine();
                while (string.IsNullOrWhiteSpace(name) || !name.All(char.IsLetter))
                {
                    Console.WriteLine($"Please re-enter a valid name instead of {name}");
                    name = Console.ReadLine();
                }
                employees[id].Employee_name = name;
                break;
            case 2:
                double Salary;
                System.Console.WriteLine("Please Enter Salary");
                while (!double.TryParse(Console.ReadLine(), out Salary))
                {
                    System.Console.WriteLine("Invalid salary , Re-enter the salary please");
                }
                employees[id].salary = Salary;
                break;
            case 3:
                int Age;
                System.Console.WriteLine("Please Enter Age");
                while (!int.TryParse(Console.ReadLine(), out Age))
                {
                    System.Console.WriteLine("Invalid age, Re-enter the age please");
                }
                employees[id].age = Age;
                break;
            default:
                System.Console.WriteLine("Invalid choice");
                break;

        }


        System.Console.WriteLine("Employee modified successfully ");

        System.Console.WriteLine("The employee details are");
        System.Console.WriteLine($"Name : {employees[id].Employee_name}");
        System.Console.WriteLine($"Salary : {employees[id].salary}");
        System.Console.WriteLine($"Age : {employees[id].age}");
        System.Console.WriteLine("want to modify any other field? (y/n)");
        string? choice1 = Console.ReadLine();
        if (choice1 == "y")
        {
            ModifyEmployeeByID(id);
        }
        else
        {
            System.Console.WriteLine("Exiting the modify employee");
        }
    }

    // 7. Delete employee by ID
    private static void DeleteEmployeeId(int id)
    {
        if (!employees.ContainsKey(id))
        {
            System.Console.WriteLine("Employee Not found");
            return;
        }
        employees.Remove(id);
        System.Console.WriteLine("Employee deleted successfully");
    }

    public static void Main()
    {

        while (true)
        {
            System.Console.WriteLine("\n\nChoices \n 1.Input employees  \n 2.Get the employee by ID\n 3.Display Employees based on salary  \n 4.Display all the employees in given name \n 5.Display all the employees who are elder than given employee \n 6.Modify employee by ID \n 7.Delete employee by ID \n 8.Exit");
            System.Console.WriteLine("Please enter Your choice");
            int choice;
            int.TryParse(Console.ReadLine(), out choice);
            switch (choice)
            {
                case 1:
                    InsertEmployees();
                    break;

                
                case 2:
                    System.Console.WriteLine("Please enter the Employee ID");
                    int id = 0;
                    while (!int.TryParse(Console.ReadLine(), out id))
                    {
                        System.Console.WriteLine("Invalid ID");
                    }
                    DisplayEmployeeByID(id);
                    break;


                case 3:
                    SortEmployeesbySalary();
                    break;


                case 4:
                    System.Console.WriteLine("Please enter the Employee name");
                    string name = Console.ReadLine();
                    while (string.IsNullOrWhiteSpace(name) || !name.All(char.IsLetter))
                    {
                        Console.WriteLine($"Please re-enter a valid name instead of {name}");
                        name = Console.ReadLine();
                    }
                    DisplayAllEmployeesWithGivenName(name);
                    break;


                case 5:
                    System.Console.WriteLine("Please enter the Employee ID");
                    int id3;
                    while (!int.TryParse(Console.ReadLine(), out id3))
                    {
                        System.Console.WriteLine("Invalid id, Re-enter the id please");
                    }
                    DisplayAllEmployeesAboveGivenAge(id3);
                    break;


                case 6:
                    System.Console.WriteLine("Please enter the Employee ID");
                    int id1 = 0;
                    while (!int.TryParse(Console.ReadLine(), out id1))
                    {
                        System.Console.WriteLine("Invalid ID");
                    }
                    ModifyEmployeeByID(id1);
                    break;


                case 7:
                    System.Console.WriteLine("Please enter the Employee ID");
                    int id2 = 0;
                    while (!int.TryParse(Console.ReadLine(), out id2))
                    {
                        System.Console.WriteLine("Invalid ID");
                    }
                    DeleteEmployeeId(id2);
                    break;


                case 8:

                    System.Console.WriteLine("Exiting the program.");
                    return;


                default:
                    System.Console.WriteLine("Invalid choice");
                    break;

            }

        }



    }
}
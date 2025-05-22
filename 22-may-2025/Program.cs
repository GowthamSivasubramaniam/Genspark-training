
namespace WholeApplication
{
    public class Employee
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
        
        public override string ToString()
        {
            return $"Employee ID : {Id}\nName : {Name}\nAge : {Age}\nSalary : {Salary}";
        }

    }
    internal class Program
    {
        List<Employee> employees = new List<Employee>()
        {
            new Employee{Id = 101, Age=30, Name= "John Doe",  Salary = 50000 },
            new Employee{Id = 102, Age = 25, Name = "Jane Smith", Salary = 60000},
            new Employee{Id = 103, Age = 35, Name = "Sam Brown", Salary = 70000}
        };

        public delegate void MyDelegate(int num1, int num2);

        public void Add(int n1, int n2)
        {
            int sum = n1 + n2;
            Console.WriteLine($"The sum of {n1} and {n2} is {sum}");
        }
        public void Product(int n1, int n2)
        {
            int prod = n1 * n2;
            Console.WriteLine($"The Product of {n1} and {n2} is {prod}");
        }

         void FindEmployee()
        {
            int empId = 102;
            Predicate<Employee> predicate = e => e.Id == empId;
            Employee? emp = employees.Find(predicate);
            Console.WriteLine(emp.ToString() ?? "No such employee");
        }
        void SortEmployee()
        {
            Func<Employee, string> func = e => e.Name;
            var sortedEmployees = employees.OrderBy(func);
            foreach (var emp in sortedEmployees)
            {
                Console.WriteLine(emp);
            }
        }
        // Program()
        // {
        //     MyDelegate del = new MyDelegate(Add);
        //     del += Product;
        //     del(10, 20);
        //     Action<int, int> de1 = Add;
        //     de1 += Product;
        //     de1 += (n1, n2) => Console.WriteLine($"Division of  {n1} and {n2} is {n1 / n2}");
        //     de1 += delegate (int n1, int n2)
        //     {
        //         Console.WriteLine($"Subtraction of {n1} and {n2} is {n1 - n2}");
        //     };
        //     de1(10, 20);
        // }
        static void Main(string[] args)
        {
            new Program();
            Program p = new Program();
            p.FindEmployee();
            p.SortEmployee();
            

            
        }
    }
}

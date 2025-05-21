using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

	
    public interface IRepositor<K,T> where T : class
    {

        T Add(T item);
        T Update(T item);
        T Delete(K id);
        T GetById(K id);
        ICollection<T> GetAll();
    }


    public abstract class Repository<K,T> :IRepositor<K, T> where T : class
    {
        protected List<T> _items = new List<T>();
        protected abstract K GenerateID();
        public abstract ICollection<T> GetAll();
        public abstract T GetById(K id);

        public T Add(T item)
        {
            var id = GenerateID();
            var property = typeof(T).GetProperty("Id");
            if (property != null)
            {
                property.SetValue(item, id);
            }
           
            if (_items.Contains(item))
            {
                throw new DuplicateEntityException("Employee already exists");
            }
            _items.Add(item);
            return item;
        }

        public T Delete(K id)
        {
            var item = GetById(id);
            if (item == null)
            {
                throw new KeyNotFoundException("Item not found");
            }
            _items.Remove(item);
            return item;
        }

        public T Update(T item)
        {
            var myItem = GetById((K)item.GetType().GetProperty("Id").GetValue(item));
            if (myItem == null)
            {
                throw new KeyNotFoundException("Item not found");
            }
            var index = _items.IndexOf(myItem);
            _items[index] = item;
            return item;
        }
    }


    
   
    public class EmployeeRepository : Repository<int, Employee>
    {
        public EmployeeRepository() : base()
        {
        }
        public override ICollection<Employee> GetAll()
        {
            if(_items.Count == 0)
            {
                throw new CollectionEmptyException("No employees found");
            }
            return _items;
        }
        public override Employee GetById(int id)
        {
            var employee = _items.FirstOrDefault(e => e.Id == id);
            if(employee == null)
            {
                throw new KeyNotFoundException("Employee not found");
            }
            return employee;
        }
 
        protected override int GenerateID()
        {
            if(_items.Count == 0)
            {
                return 101;
            }
            else
            {
                return _items.Max(e => e.Id) + 1;
            }
        }
       
 
    }
    

     public interface IEmployeeService
    {
        int AddEmployee(Employee employee);
        List<Employee>? SearchEmployee(SearchModel searchModel);
    }



    public class EmployeeService : IEmployeeService
{
    // IRepositor<int, Employee> _employeeRepository;
    // public EmployeeService(IRepositor<int, Employee> employeeRepository)
    // {
    //     _employeeRepository = employeeRepository;
    // }


    public static EmployeeRepository _employeeRepository = new EmployeeRepository();



    public int AddEmployee(Employee employee)
    {
        try
        {
            var result = _employeeRepository.Add(employee);
            if (result != null)
            {
                return result.Id;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return -1;
    }

    public List<Employee>? SearchEmployee(SearchModel searchModel)
    {
        try
        {
            var employees = _employeeRepository.GetAll();
            employees = SearchById(employees, searchModel.Id);
            employees = SearchByName(employees, searchModel.Name);
            employees = SeachByAge(employees, searchModel.Age);
            employees = SearchBySalary(employees, searchModel.Salary);
            if (employees != null && employees.Count > 0)
                return employees.ToList(); ;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return null;
    }

    private ICollection<Employee> SearchBySalary(ICollection<Employee> employees, Range<double>? salary)
    {
        if (salary == null || employees.Count == 0 || employees == null)
        {
            return null;
        }
        else
        {
            return employees.Where(e => e.Salary >= salary.MinVal && e.Salary <= salary.MaxVal).ToList();
        }
    }

    private ICollection<Employee> SeachByAge(ICollection<Employee> employees, Range<int>? age)
    {
        if (age == null || employees.Count == 0 || employees == null)
        {
            return null;
        }
        else
        {
            return employees.Where(e => e.Age >= age.MinVal && e.Age <= age.MaxVal).ToList();
        }
    }

    private ICollection<Employee> SearchByName(ICollection<Employee> employees, string? name)
    {
        if (name == null || employees.Count == 0 || employees == null)
        {
            return null;
        }
        else
        {
            return employees.Where(e => e.Name.ToLower().Contains(name.ToLower())).ToList();
        }
    }

    private ICollection<Employee> SearchById(ICollection<Employee> employees, int? id)
    {
        if (id == null || employees.Count == 0 || employees == null)
        {
            return null;
        }
        else
        {
            return employees.Where(e => e.Id == id).ToList();
        }
    }
}




    public class SearchModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public Range<int>? Age { get; set; }
        public Range<double>? Salary { get; set; }
    }
    public class Range<T>
    {
        public T? MinVal { get; set; }
        public T? MaxVal { get; set; }
    }




 


    public class CollectionEmptyException :Exception
    {
        private string _message = "Collection is empty";
        public CollectionEmptyException(string msg)
        {
            _message = msg;
        }
        public override string Message => _message;
    }


    public class DuplicateEntityException : Exception
    {
        private string _message = "Duplicate entity found";
        public DuplicateEntityException(string msg)
        {
            _message = msg;
        }
        public override string Message => _message;
    }



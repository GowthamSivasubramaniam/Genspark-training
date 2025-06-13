using Microsoft.EntityFrameworkCore;
using VSM.Contexts;
using VSM.Models;

namespace VSM.Repositories
{
    public class CustomerRepository : Repository<Guid, Customer>
    {

        public CustomerRepository(VSMContext context) : base(context) { }
        public override async Task<Customer> Get(Guid key)
        {
            var user =await _context.Customers.SingleOrDefaultAsync(u => u.CustomerID == key);
            if (user == null)
                throw new Exception("Mechanic Not Found");
            return user;
            
        }

        public override async Task<IEnumerable<Customer>> GetAll(int pageNumber,int pageSize)
        {
            return await _context.Customers.Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();
        }
    }
}
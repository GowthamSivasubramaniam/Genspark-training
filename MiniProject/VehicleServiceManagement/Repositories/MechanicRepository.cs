using Microsoft.EntityFrameworkCore;
using VSM.Contexts;
using VSM.Models;

namespace VSM.Repositories
{
    public class MechanicRepository : Repository<Guid, Mechanic>
    {

        public MechanicRepository(VSMContext context) : base(context) { }
        public override async Task<Mechanic> Get(Guid key)
        {
            var user =await _context.Mechanics.SingleOrDefaultAsync(u => u.MechanicId == key);
            if (user == null)
                throw new Exception("Mechanic Not Found");
            return user;
            
        }

        public override async Task<IEnumerable<Mechanic>> GetAll(int pageNumber,int pageSize)
        {
            return await _context.Mechanics.Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();
        }
    }
}
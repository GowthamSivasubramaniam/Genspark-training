using Microsoft.EntityFrameworkCore;
using VSM.Contexts;
using VSM.Models;

namespace VSM.Repositories
{
    public class UserRepository : Repository<string, User>
    {

        public UserRepository(VSMContext context) : base(context) { }
        public override async Task<User> Get(string key)
        {
            var user =await _context.Users.SingleOrDefaultAsync(u => u.Email == key);
            return user;
            
        }

        public override async Task<IEnumerable<User>> GetAll(int pageNumber,int pageSize)
        {
            return await _context.Users.Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();
        }
    }
}
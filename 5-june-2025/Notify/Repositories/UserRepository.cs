using Notify.Contexts;
using Notify.Interfaces;
using Notify.Models;

using Microsoft.EntityFrameworkCore;

namespace Notify.Repositories
{
    public class UserRepository : Repository<string, User>
    {
        public UserRepository(NotifyContext context):base(context)
        {
            
        }
        public override async Task<User> Get(string key)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Mail == key);
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }
            
    }
}
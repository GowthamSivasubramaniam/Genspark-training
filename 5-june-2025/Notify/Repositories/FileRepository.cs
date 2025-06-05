using Notify.Contexts;
using Notify.Interfaces;
using Notify.Models;
using Notify.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Notify.Repositories
{
    public class FileRepository : Repository<string, Files>
    {
        public FileRepository(NotifyContext context):base(context)
        {
            
        }
        public override async Task<Files> Get(string key)
        {
            return await _context.files.SingleOrDefaultAsync(u => u.Umail == key);
        }

        public override async Task<IEnumerable<Files>> GetAll()
        {
            return await _context.files.ToListAsync();
        }
            
    }
}
using Microsoft.EntityFrameworkCore;
using VSM.Contexts;
using VSM.Models;

namespace VSM.Repositories
{
    public class ServiceRepository : Repository<Guid, Service>
    {

        public ServiceRepository(VSMContext context) : base(context) { }
        public override async Task<Service> Get(Guid key)
        {
            var service = await _context.Services
                .Include(s => s.ServiceCategories)
                .SingleOrDefaultAsync(u => u.ServiceID == key);
            if (service == null)
                throw new Exception("Service Not Found");
            return service;
        }

        public override async Task<IEnumerable<Service>> GetAll(int pageNumber, int pageSize)
        {
            return await _context.Services
                .Include(s => s.ServiceCategories)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
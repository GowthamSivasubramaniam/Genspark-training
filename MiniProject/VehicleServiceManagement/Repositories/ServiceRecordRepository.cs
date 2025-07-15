using Microsoft.EntityFrameworkCore;
using VSM.Contexts;
using VSM.Models;

namespace VSM.Repositories
{
    public class ServiceRecordRepository : Repository<Guid, ServiceRecord>
    {
        public ServiceRecordRepository(VSMContext context) : base(context) { }

        public override async Task<ServiceRecord> Get(Guid key)
        {
            var serviceRecord = await _context.serviceRecords
                .Include(sr => sr.Service)
                    .ThenInclude(s => s.ServiceCategories)
                .SingleOrDefaultAsync(u => u.ServiceRecordID == key);

            if (serviceRecord == null)
                throw new Exception("Item Not Found");
            return serviceRecord;
        }

        public override async Task<IEnumerable<ServiceRecord>> GetAll(int pageNumber, int pageSize, string? search = null)
        {
            var query = _context.serviceRecords
                .Include(c => c.Customer)
                .Include(m => m.Mechanic)
                .Include(sr => sr.Service)
                    .ThenInclude(s => s.ServiceCategories)
                  .Include(sr => sr.Service)
                    .ThenInclude(s => s.Vehicle)
                .Where(v => !v.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s => s.Customer.Phone.Contains(search) || s.Mechanic.Phone.Contains(search));
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
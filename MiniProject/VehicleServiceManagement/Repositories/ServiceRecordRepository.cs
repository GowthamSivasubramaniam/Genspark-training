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

        public override async Task<IEnumerable<ServiceRecord>> GetAll(int pageNumber, int pageSize)
        {
            return await _context.serviceRecords
                .Include(sr => sr.Service)
                    .ThenInclude(s => s.ServiceCategories)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
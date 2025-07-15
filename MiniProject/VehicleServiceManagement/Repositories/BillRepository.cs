using Microsoft.EntityFrameworkCore;
using VSM.Contexts;
using VSM.Models;

namespace VSM.Repositories
{
    public class BillRepository : Repository<Guid, Bill>
    {

        public BillRepository(VSMContext context) : base(context) { }
        public override async Task<Bill> Get(Guid key)
        {
            var bill = await _context.Bills
                .Include(b => b.CategoryDetails)
                .Include(b => b.ServiceRecord)
                    .ThenInclude(sr => sr.Service)
                        .ThenInclude(s => s.ServiceCategories)
                .Include(b => b.ServiceRecord)
                    .ThenInclude(sr => sr.Customer)
                .SingleOrDefaultAsync(b => b.BillID == key);

            if (bill == null)
                throw new Exception("Bill Not Found");
            return bill;
        }

        public override async Task<IEnumerable<Bill>> GetAll(int pageNumber, int pageSize, string? search = null)
        {
            var query = _context.Bills
                 .Include(b => b.CategoryDetails)
                 .Include(b => b.ServiceRecord)
                     .ThenInclude(sr => sr.Service)
                         .ThenInclude(s => s.ServiceCategories)
                 .Include(b => b.ServiceRecord)
                     .ThenInclude(sr => sr.Service)
                         .ThenInclude(s => s.Vehicle)
                 .Include(b => b.ServiceRecord)
                     .ThenInclude(sr => sr.Customer)
                 .Include(b => b.ServiceRecord)
                     .ThenInclude(sr => sr.Mechanic).AsQueryable();
                 
                
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s => s.ServiceRecord.Customer.Phone.Contains(search) || s.ServiceRecord.Mechanic.Phone.Contains(search));
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
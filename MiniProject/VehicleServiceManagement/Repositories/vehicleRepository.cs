using Microsoft.EntityFrameworkCore;
using VSM.Contexts;
using VSM.Models;

namespace VSM.Repositories
{
    public class VehicleRepository : Repository<Guid, Vehicle>
    {

        public VehicleRepository(VSMContext context) : base(context) { }
        public override async Task<Vehicle> Get(Guid key)
        {
            var vehicle = await _context.Vehicles.SingleOrDefaultAsync(u => u.VehicleID == key);
            if (vehicle == null)
                throw new Exception("Vehicle Not Found");
            return vehicle;

        }

        public override  async Task<IEnumerable<Vehicle>> GetAll(int pageNumber, int pageSize, string? search=null)
        {
            var query = _context.Vehicles.AsQueryable();

            
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(v => v.VehicleNo.ToLower().Contains(search.ToLower()));
            }

           
            query = query.Where(v => !v.IsDeleted);

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
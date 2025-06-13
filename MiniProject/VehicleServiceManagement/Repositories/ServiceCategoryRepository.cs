using Microsoft.EntityFrameworkCore;
using VSM.Contexts;
using VSM.Models;

namespace VSM.Repositories
{
    public class ServiceCategoriesRepository : Repository<Guid, ServiceCategory>
    {

        public ServiceCategoriesRepository(VSMContext context) : base(context) { }
        public override async Task<ServiceCategory> Get(Guid key)
        {
            var category =await _context.ServiceCategories.SingleOrDefaultAsync(u => u.CategoryID == key);
            if (category == null)
                throw new Exception("category Not Found");
            return category;
            
        }

        public override async Task<IEnumerable<ServiceCategory>> GetAll(int pageNumber,int pageSize)
        {
            return await _context.ServiceCategories.Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();
        }
    }
}
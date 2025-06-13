using Microsoft.EntityFrameworkCore;
using VSM.Contexts;
using VSM.Models;

namespace VSM.Repositories
{
    public class BookingRepository : Repository<Guid, Booking>
    {

        public BookingRepository(VSMContext context) : base(context) { }
        public override async Task<Booking> Get(Guid key)
        {
            var booking =await _context.Bookings.SingleOrDefaultAsync(u => u.BookingID == key);
            if (booking == null)
                throw new Exception("Booking Not Found");
            return booking;
            
        }

        public override async Task<IEnumerable<Booking>> GetAll(int pageNumber,int pageSize)
        {
            return await _context.Bookings.Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();
        }
    }
}
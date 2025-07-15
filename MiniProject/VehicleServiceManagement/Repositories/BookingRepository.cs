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
            var booking = await _context.Bookings.SingleOrDefaultAsync(u => u.BookingID == key);
            if (booking == null)
                throw new Exception("Booking Not Found");
            return booking;

        }

        public override async Task<IEnumerable<Booking>> GetAll(int pageNumber, int pageSize, string? search = null)
        {
            return await _context.Bookings
                .Include(b => b.Customer)
                .OrderBy(b => b.Status == "Booked" ? 0 :
                            b.Status == "Reviewed" ? 1 :
                            b.Status == "Cancelled" ? 2 : 3)
                .ToListAsync();

        }
    }
}
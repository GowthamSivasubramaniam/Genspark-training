using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VSM.Contexts;
using VSM.DTO;

[Route("api/v1/[controller]")]
[ApiController]
public class AnalyticsController : ControllerBase
{
    private readonly VSMContext _context;

    public AnalyticsController(VSMContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin,Mechanic,Customer")]
    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardAnalyticsDto>> GetDashboardStats(
        [FromQuery] string? phone = null,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null)
    {

        Console.WriteLine(phone);
        if (from.HasValue && to.HasValue && from > to)
            return BadRequest("'from' date must be less than or equal to 'to' date.");
        if (from.HasValue)
    from = DateTime.SpecifyKind(from.Value, DateTimeKind.Utc);

if (to.HasValue)
    to = DateTime.SpecifyKind(to.Value, DateTimeKind.Utc);

        var dto = new DashboardAnalyticsDto();

       
        var bookings = _context.Bookings.Include(b => b.Customer).AsQueryable();
        var bills = _context.Bills
                            .Include(b => b.ServiceRecord)
                            .ThenInclude(sr => sr.Customer)
                            .Include(b => b.ServiceRecord)
                            .ThenInclude(sr => sr.Mechanic)
                            .AsQueryable();
        var services = _context.serviceRecords
                               .Include(s => s.Mechanic)
                               .Include(s => s.Customer)
                               .Include(s => s.Booking)
                               .AsQueryable();
        var customers = _context.Customers.AsQueryable();
        var mechanics = _context.Mechanics.AsQueryable();

        
        if (!string.IsNullOrEmpty(phone))
        {
            bookings = bookings.Where(b => b.Customer.Phone == phone);
            services = services.Where(s =>
                (s.Customer != null && s.Customer.Phone == phone) ||
                (s.Mechanic != null && s.Mechanic.Phone == phone));
            bills = bills.Where(b => b.ServiceRecord != null &&
                                    ((b.ServiceRecord.Customer != null && b.ServiceRecord.Customer.Phone == phone) ||
                                     (b.ServiceRecord.Mechanic != null && b.ServiceRecord.Mechanic.Phone == phone)));
            customers = customers.Where(c => c.Phone == phone);
            mechanics = mechanics.Where(m => m.Phone == phone);
        }

       
        if (from.HasValue && to.HasValue)
        {
            bookings = bookings.Where(b => b.Slot >= from && b.Slot <= to);
            services = services.Where(s => s.Booking != null && s.Booking.Slot >= from && s.Booking.Slot <= to);
        }

        // Customer stats
        dto.TotalCustomers = await customers.CountAsync();
        dto.ActiveCustomers = await customers.CountAsync(c => c.Status == "Active");

        // Mechanic stats
        dto.TotalMechanics = await mechanics.CountAsync();
        dto.ActiveMechanics = await mechanics.CountAsync(m => m.Status == "Active");

        // Booking stats
        dto.TotalBookings = await bookings.CountAsync();
        dto.ActiveBookings = await bookings.CountAsync(b => b.Status == "Booked");
        dto.CancelledBookings = await bookings.CountAsync(b => b.Status == "Cancelled");
        dto.ReviewedBookings = await bookings.CountAsync(b => b.Status == "Reviewed");

        // Bill stats
        dto.TotalBills = await bills.CountAsync();
        dto.DispatchedBills = await bills.CountAsync(b => b.Status == "Dispatched");
        dto.ApprovedBills = await bills.CountAsync(b => b.Status == "Approved");
        dto.PaidBills = await bills.CountAsync(b => b.Status == "Paid");

        // Service stats
        dto.TotalServices = await services.CountAsync();
        dto.ActiveServices = await services.CountAsync(s => s.Status == "Active");
        dto.CompletedServices = await services.CountAsync(s => s.Status == "Completed");
        dto.AbortedServices = await services.CountAsync(s => s.Status == "Aborted");

       
        dto.CustomerBillSummary = await bills
            .Where(b => b.ServiceRecord != null && b.ServiceRecord.Customer != null)
            .GroupBy(b => b.ServiceRecord.Customer.Phone)
            .Select(g => new { Phone = g.Key!, Total = g.Sum(b => b.Amount) })
            .ToDictionaryAsync(x => x.Phone, x => x.Total);

       
        dto.MechanicServiceStats = await services
            .Where(s => s.Mechanic != null)
            .GroupBy(s => new { s.Mechanic.Phone, s.Mechanic.Name })
            .Select(g => new MechanicServiceStatsDto
            {
                MechanicPhone = g.Key.Phone!,
                MechanicName = g.Key.Name!,
                Active = g.Count(x => x.Status == "Active"),
                Completed = g.Count(x => x.Status == "Completed"),
                Aborted = g.Count(x => x.Status == "Aborted")
            })
            .ToListAsync();

      
        dto.BookingCountsByDate = await bookings
            .GroupBy(b => b.Slot.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Date, x => x.Count);

      
        dto.ServiceCountsByDate = await services
            .Where(s => s.Booking != null)
            .GroupBy(s => s.Booking.Slot.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Date, x => x.Count);

        return Ok(dto);
    }
}

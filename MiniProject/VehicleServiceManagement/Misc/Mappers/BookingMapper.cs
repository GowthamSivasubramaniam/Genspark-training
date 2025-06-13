using VSM.DTO;
using VSM.Models;

namespace VSM.Misc.Mappers
{
    public class BookingMapper
    {
        public Booking MapBooking(BookingAddDto dto , string path)
        {
            const string slotFormat = "dd/MM/yyyy HH:00";
            if (!DateTime.TryParseExact(dto.Slot, slotFormat, null,
                System.Globalization.DateTimeStyles.None, out var parsedSlot))
            {
                throw new Exception("Invalid Slot format. Expected format: '10/06/2025 09:00'");
            }
             parsedSlot = DateTime.SpecifyKind(parsedSlot, DateTimeKind.Utc);
            return new Booking
            {
                Slot = parsedSlot,
                CustomerID = dto.CustomerID,
                Status = "Booked",
                Imageurl = path,
                BookedAt = DateTime.UtcNow,
                DeliveryTime = parsedSlot.AddDays(2)
            };
        }

        public BookingDisplayDto MapToDisplayDto(Booking booking)
        {
            return new BookingDisplayDto
            {
                BookingID = booking.BookingID,
                CustomerID = booking.CustomerID,
                BookedAt = booking.BookedAt,
                Slot = booking.Slot.ToString("dd/MM/yyyy HH:00"),
                DeliveryTime = booking.DeliveryTime,
                Status = booking.Status,
                ImageUrl = booking.Imageurl
            };
        }

        public IEnumerable<BookingDisplayDto> MapToDisplayDtos(IEnumerable<Booking> bookings)
        {
            return bookings.Select(MapToDisplayDto);
        }
    }
}
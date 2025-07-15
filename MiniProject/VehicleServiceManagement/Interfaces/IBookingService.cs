using VSM.DTO;
using VSM.Models;

namespace VSM.Interfaces
{
    public interface IBookingService
    {
        Task<BookingDisplayDto> CreateBooking(BookingAddDto dto);
        Task<List<string>> DisplaySlots();
        Task<BookingDisplayDto> GetById(Guid id);
        Task<IEnumerable<Booking>> GetAll();
        Task<IEnumerable<Booking>> GetAllBookings();
        Task<BookingDisplayDto?> UpdateBooking(Guid id);
        Task<BookingDisplayDto?> CancelBooking(Guid id);
    }
}

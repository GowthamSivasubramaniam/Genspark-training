using VSM.DTO;
using VSM.Models;

namespace VSM.Interfaces
{
    public interface IBookingService
    {
        Task<BookingDisplayDto> CreateBooking(BookingAddDto dto);
        Task<List<string>> DisplaySlots();
        Task<BookingDisplayDto> GetById(Guid id);
        Task<IEnumerable<BookingDisplayDto>> GetAll();
        Task<BookingDisplayDto?> UpdateBooking(Guid id, DateTime slot);
        Task<BookingDisplayDto?> CancelBooking(Guid id);
    }
}

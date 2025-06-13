using VSM.Interfaces;
using VSM.Models;
using VSM.DTO;
using VSM.Misc.Mappers;

namespace VSM.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<Guid, Booking> _bookingRepo;
        private readonly IRepository<Guid, Customer> _cRepo;
        private readonly BookingMapper _mapper;

        public BookingService(IRepository<Guid, Booking> bookingRepo, IRepository<Guid, Customer> cRepo)
        {
            _bookingRepo = bookingRepo;
            _cRepo = cRepo;
            _mapper = new BookingMapper();
        }
        

        public async Task<BookingDisplayDto> CreateBooking(BookingAddDto dto)
        {
            var allowedSlots = await DisplaySlots();
            var customer = await _cRepo.Get(dto.CustomerID);
            if (customer == null)
                throw new Exception("Invalid customerID");


            const string slotFormat = "dd/MM/yyyy HH:00";


            if (!DateTime.TryParseExact(dto.Slot, slotFormat, null, System.Globalization.DateTimeStyles.None, out var requestedSlot))
            {
                throw new Exception("Invalid time slot. Please use format like '10/06/2025 09:00'.");
            }

           
            var allowedDateTimes = allowedSlots
            .Select(slot => DateTime.ParseExact(slot, slotFormat, null))
            .ToList();

            if (!allowedDateTimes.Contains(requestedSlot))
            {
                throw new Exception("Invalid time slot. Please choose an available slot.");
            }
            string? imagePath = null;
            if (dto.Image != null && dto.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "BookingImages");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = $"{Guid.NewGuid()}_{dto.Image.FileName}";
                imagePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }
            }
            var bookings = await _bookingRepo.GetAll(1, 100);

            bool isExisting = bookings.Any(b => b.CustomerID == dto.CustomerID && b.Slot == requestedSlot);

            if (isExisting)
            {
                throw new Exception("Customer already has a booking for this slot.");
            }

            Console.WriteLine(isExisting);
            if (isExisting)
                throw new Exception("You have already booked this slot");
            var count = bookings.Count(b => b.Slot == requestedSlot && !b.IsDeleted && b.Status != "Cancelled");

            if (count >= 5)
            {
                throw new Exception("Slot already fully booked. Please choose a different time.");
            }

            var booking = _mapper.MapBooking(new BookingAddDto { Slot = dto.Slot, CustomerID = dto.CustomerID }, imagePath);

            var added = await _bookingRepo.Add(booking) ?? throw new Exception("Booking creation failed");
            return _mapper.MapToDisplayDto(added);
        }

        public async Task<List<string>> DisplaySlots()
        {
            var bookings = await _bookingRepo.GetAll(1,100);

            var startDate = DateTime.UtcNow.Date;
            var endDate = startDate.AddDays(2);

            List<DateTime> availableSlots = new();

            for (int dayOffset = 0; dayOffset < 2; dayOffset++)
            {
                var date = startDate.AddDays(dayOffset);
                for (int i = 0; i < 10; i++)
                {
                    var slot = date.AddHours(9 + i);
                    int bookingCount = bookings.Count(b => !b.IsDeleted && b.Slot == slot && b.Status != "Cancelled");
                    if (bookingCount < 5)
                    {
                        availableSlots.Add(slot);
                    }
                }
            }

            return availableSlots
                .OrderBy(s => s)
                .Select(s => s.ToString("dd/MM/yyyy HH:00"))
                .ToList();
        }

        public async Task<BookingDisplayDto> GetById(Guid id)
        {
            var booking = await _bookingRepo.Get(id) ?? throw new Exception("Booking not found");
            return _mapper.MapToDisplayDto(booking);
        }

        public async Task<IEnumerable<BookingDisplayDto>> GetAll()
        {
            var bookings = await _bookingRepo.GetAll(1,100);
            var filtered = bookings.Where(b => !b.IsDeleted);
            if(filtered.Count()==0) throw new Exception("No Bookings Found");
            return _mapper.MapToDisplayDtos(filtered);
        }

        public async Task<BookingDisplayDto?> UpdateBooking(Guid id, DateTime slot)
        {
            
            var booking = await _bookingRepo.Get(id) ?? throw new Exception("Booking not found");
            if(booking.Status =="cancelled")
            slot = DateTime.SpecifyKind(slot, DateTimeKind.Utc);

            booking.Slot = slot;
            booking.DeliveryTime = slot.AddDays(5);
            booking.Status = "Rescheduled";

            var updated = await _bookingRepo.Update(id, booking);
            return _mapper.MapToDisplayDto(updated);
        }
        public async Task<BookingDisplayDto?> CancelBooking(Guid id)
        {
            var booking = await _bookingRepo.Get(id) ?? throw new Exception("Booking not found");
            booking.Status = "Cancelled";

            var updated = await _bookingRepo.Update(id, booking);
            return _mapper.MapToDisplayDto(updated);
        }


    }
}
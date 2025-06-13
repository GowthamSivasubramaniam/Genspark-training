using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VSM.DTO;
using VSM.Interfaces;
using VSM.Misc;

namespace VSM.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IFileLogger _logger;

        public BookingController(IBookingService bookingService, IFileLogger logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<BookingDisplayDto>> CreateBooking([FromForm] BookingAddDto dto)
        {
            try
            {
                var result = await _bookingService.CreateBooking(dto);
                _logger.LogData($"Booking Created {result.BookingID}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Creating Booking", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
         [Authorize(Roles = "Admin,Customer")]
        [HttpGet("slots")]
        public async Task<ActionResult<List<string>>> GetAvailableSlots()
        {
            try
            {
                var slots = await _bookingService.DisplaySlots();
                return Ok(slots);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Getting Available Slots", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
         [Authorize(Roles = "Admin,Customer")]
          [HttpGet("{id}")]
        public async Task<ActionResult<BookingDisplayDto>> GetById(Guid id)
        {
            try
            {
                var booking = await _bookingService.GetById(id);
                return Ok(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Getting Booking By Id", ex);
                return NotFound(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Mechanic")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDisplayDto>>> GetAll()
        {
            try
            {
                var bookings = await _bookingService.GetAll();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Getting All Bookings", ex);
                return BadRequest(new { message = ex.Message });
            }
        }

        //  [HttpPut("{id}")]
        //  [Authorize(Roles = "Admin,Customer")]
        // public async Task<ActionResult<BookingDisplayDto>> UpdateBooking(Guid id, [FromQuery] DateTime slot)
        // {
        //     try
        //     {
        //         var updated = await _bookingService.UpdateBooking(id, slot);
        //         _logger.LogData($"Booking Updated {id}");
        //         return Ok(updated);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError("Error Updating Booking", ex);
        //         return BadRequest(new { message = ex.Message });
        //     }
        // }
         [HttpPut("/Cancel/{id}")]
         [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<BookingDisplayDto>> CancelBooking(Guid id)
        {
            try
            {
                var updated = await _bookingService.CancelBooking(id);
                _logger.LogData($"Booking Cancelled {id}");
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Cancelling Booking", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
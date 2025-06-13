using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using VSM.DTO;
using VSM.Interfaces;
using VSM.Models;
using VSM.Services;
using Xunit;

public class BookingServiceTests
{
    private readonly Mock<IRepository<Guid, Booking>> _bookingRepoMock = new();
    private readonly Mock<IRepository<Guid, Customer>> _customerRepoMock = new();
    private readonly BookingService _bookingService;

    public BookingServiceTests()
    {
        _bookingService = new BookingService(_bookingRepoMock.Object, _customerRepoMock.Object);
    }

    [Fact]
    public async Task CreateBooking_Should_Throw_Exception_For_Invalid_Customer()
    {
        var dto = new BookingAddDto { CustomerID = Guid.NewGuid(), Slot = "11/06/2025 09:00" };
        _customerRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((Customer)null);

        var ex = await Assert.ThrowsAsync<Exception>(() => _bookingService.CreateBooking(dto));
        Assert.Equal("Invalid customerID", ex.Message);
    }

    [Fact]
    public async Task CreateBooking_Should_Throw_Exception_For_Invalid_Slot_Format()
    {
        var dto = new BookingAddDto { CustomerID = Guid.NewGuid(), Slot = "invalid format" };
        _customerRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(new Customer());

        var ex = await Assert.ThrowsAsync<Exception>(() => _bookingService.CreateBooking(dto));
        Assert.Contains("Invalid time slot", ex.Message);
    }

    [Fact]
    public async Task CreateBooking_Should_Throw_Exception_For_Slot_Not_Allowed()
    {
        var dto = new BookingAddDto { CustomerID = Guid.NewGuid(), Slot = "11/06/2025 09:00" };

        _customerRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(new Customer());
        _bookingRepoMock.Setup(r => r.GetAll(1, 100)).ReturnsAsync(new List<Booking>());

        // fake DisplaySlots will return only a different slot
        var service = new BookingService(_bookingRepoMock.Object, _customerRepoMock.Object);
        var ex = await Assert.ThrowsAsync<Exception>(() => service.CreateBooking(dto));
        Assert.NotNull(ex.Message);
    }

    

    // [Fact]
    // public async Task CreateBooking_Should_Succeed()
    // {
    //     var slot = DateTime.UtcNow.Date.AddHours(2);
    //     var slotStr = slot.ToString("dd/MM/yyyy HH:00");

    //     var dto = new BookingAddDto { CustomerID = Guid.NewGuid(), Slot = slotStr };
    //     _customerRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(new Customer());
    //     _bookingRepoMock.Setup(r => r.GetAll(1, 100)).ReturnsAsync(new List<Booking>());

    //     _bookingRepoMock.Setup(r => r.Add(It.IsAny<Booking>())).ReturnsAsync((Booking b) => b);

    //     var result = await _bookingService.CreateBooking(dto);
    //     Assert.NotNull(result);
       
    // }

    [Fact]
    public async Task GetAll_Should_Return_Filtered_Bookings()
    {
        var bookings = new List<Booking>
        {
            new Booking { Slot = DateTime.Now, IsDeleted = false },
            new Booking { Slot = DateTime.Now, IsDeleted = true }
        };

        _bookingRepoMock.Setup(r => r.GetAll(1, 100)).ReturnsAsync(bookings);

        var result = await _bookingService.GetAll();
        Assert.Single(result);
    }

    [Fact]
    public async Task GetById_Should_Throw_Exception_If_Not_Found()
    {
        _bookingRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((Booking)null);

        var ex = await Assert.ThrowsAsync<Exception>(() => _bookingService.GetById(Guid.NewGuid()));
        Assert.Equal("Booking not found", ex.Message);
    }

    [Fact]
    public async Task UpdateBooking_Should_Throw_If_Not_Found()
    {
        _bookingRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((Booking)null);

        var ex = await Assert.ThrowsAsync<Exception>(() => _bookingService.UpdateBooking(Guid.NewGuid(), DateTime.Now));
        Assert.Equal("Booking not found", ex.Message);
    }

    [Fact]
    public async Task CancelBooking_Should_Throw_If_Not_Found()
    {
        _bookingRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((Booking)null);

        var ex = await Assert.ThrowsAsync<Exception>(() => _bookingService.CancelBooking(Guid.NewGuid()));
        Assert.Equal("Booking not found", ex.Message);
    }

    [Fact]
    public async Task CancelBooking_Should_Mark_As_Cancelled()
    {
        var booking = new Booking { Slot = DateTime.Now };
        _bookingRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(booking);
        _bookingRepoMock.Setup(r => r.Update(It.IsAny<Guid>(), It.IsAny<Booking>())).ReturnsAsync(booking);

        var result = await _bookingService.CancelBooking(Guid.NewGuid());
        Assert.Equal("Cancelled", result.Status);
    }

    [Fact]
    public async Task UpdateBooking_Should_Set_Rescheduled()
    {
        var booking = new Booking { Slot = DateTime.Now };
        _bookingRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(booking);
        _bookingRepoMock.Setup(r => r.Update(It.IsAny<Guid>(), It.IsAny<Booking>())).ReturnsAsync(booking);

        var result = await _bookingService.UpdateBooking(Guid.NewGuid(), DateTime.Now.AddHours(1));
        Assert.Equal("Rescheduled", result.Status);
    }
}

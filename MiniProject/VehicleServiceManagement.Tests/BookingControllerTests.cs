using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using VSM.Controllers;
using VSM.DTO;
using VSM.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSM.Misc;
public class BookingControllerTests
{
    private readonly Mock<IBookingService> _serviceMock;
    private readonly Mock<IFileLogger> _loggerMock;
    private readonly BookingController _controller;

    public BookingControllerTests()
    {
        _serviceMock = new Mock<IBookingService>();
        _loggerMock = new Mock<IFileLogger>();
        _controller = new BookingController(_serviceMock.Object, _loggerMock.Object);
    }

    // [Fact]
    // public async Task CreateBooking_ShouldReturnOk()
    // {
    //     var dto = new BookingAddDto();
    //     var expected = new BookingDisplayDto { BookingID = Guid.NewGuid() };

    //     _serviceMock.Setup(s => s.CreateBooking(dto)).ReturnsAsync(expected);

    //     var result = await _controller.CreateBooking(dto);

    //     var ok = Assert.IsType<OkObjectResult>(result.Result);
    //     Assert.Equal(expected.BookingID, ((BookingDisplayDto)ok.Value).BookingID);
    // }

    [Fact]
    public async Task CreateBooking_ShouldReturnBadRequest_OnException()
    {
        var dto = new BookingAddDto();

        _serviceMock.Setup(s => s.CreateBooking(dto)).ThrowsAsync(new Exception("Error"));

        var result = await _controller.CreateBooking(dto);

        var bad = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Contains("Error", bad.Value.ToString());
    }

    [Fact]
    public async Task GetAvailableSlots_ShouldReturnOk()
    {
        var slots = new List<string> { "slot1", "slot2" };
        _serviceMock.Setup(s => s.DisplaySlots()).ReturnsAsync(slots);

        var result = await _controller.GetAvailableSlots();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(slots, ok.Value);
    }

    [Fact]
    public async Task GetAvailableSlots_ShouldReturnBadRequest_OnException()
    {
        _serviceMock.Setup(s => s.DisplaySlots()).ThrowsAsync(new Exception("Error"));

        var result = await _controller.GetAvailableSlots();

        var bad = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Contains("Error", bad.Value.ToString());
    }

    [Fact]
    public async Task GetById_ShouldReturnOk()
    {
        var id = Guid.NewGuid();
        var dto = new BookingDisplayDto { BookingID = id };

        _serviceMock.Setup(s => s.GetById(id)).ReturnsAsync(dto);

        var result = await _controller.GetById(id);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(id, ((BookingDisplayDto)ok.Value).BookingID);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_OnException()
    {
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetById(id)).ThrowsAsync(new Exception("Not found"));

        var result = await _controller.GetById(id);

        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Contains("Not found", notFound.Value.ToString());
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        var bookings = new List<BookingDisplayDto> { new BookingDisplayDto() };
        _serviceMock.Setup(s => s.GetAll()).ReturnsAsync(bookings);

        var result = await _controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(bookings, ok.Value);
    }

    [Fact]
    public async Task GetAll_ShouldReturnBadRequest_OnException()
    {
        _serviceMock.Setup(s => s.GetAll()).ThrowsAsync(new Exception("Error"));

        var result = await _controller.GetAll();

        var bad = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Contains("Error", bad.Value.ToString());
    }

   
    [Fact]
    public async Task CancelBooking_ShouldReturnOk()
    {
        var id = Guid.NewGuid();
        var dto = new BookingDisplayDto { BookingID = id };

        _serviceMock.Setup(s => s.CancelBooking(id)).ReturnsAsync(dto);

        var result = await _controller.CancelBooking(id);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(id, ((BookingDisplayDto)ok.Value).BookingID);
    }

    [Fact]
    public async Task CancelBooking_ShouldReturnBadRequest_OnException()
    {
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.CancelBooking(id)).ThrowsAsync(new Exception("Cancel error"));

        var result = await _controller.CancelBooking(id);

        var bad = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Contains("Cancel error", bad.Value.ToString());
    }
}

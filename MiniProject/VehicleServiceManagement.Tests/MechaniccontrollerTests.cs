using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VSM.Controllers;
using VSM.DTO;
using VSM.Interfaces;
using Xunit;
using VSM.Misc;
public class MechanicControllerTests
{
    private readonly Mock<IMechanicService> _mechanicServiceMock = new();
    private readonly Mock<IFileLogger> _loggerMock = new();
    private readonly MechanicController _controller;

    public MechanicControllerTests()
    {
        _controller = new MechanicController(_mechanicServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task RegisterMechanic_ReturnsOk_WithCreatedMechanic()
    {
        var dto = new MechanicAddDto { /* fill with test data */ };
        var displayDto = new MechanicDisplayDto { Email = "test@example.com" };

        _mechanicServiceMock.Setup(s => s.AddMechanic(dto)).ReturnsAsync(displayDto);

        var result = await _controller.RegisterMechanic(dto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedMechanic = Assert.IsType<MechanicDisplayDto>(okResult.Value);
        Assert.Equal("test@example.com", returnedMechanic.Email);

        _loggerMock.Verify(l => l.LogData(It.Is<string>(msg => msg.Contains("Registered"))), Times.Once);
    }

    [Fact]
    public async Task RegisterMechanic_OnException_ReturnsBadRequest()
    {
        var dto = new MechanicAddDto();

        _mechanicServiceMock.Setup(s => s.AddMechanic(dto)).ThrowsAsync(new Exception("fail"));

        var result = await _controller.RegisterMechanic(dto);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Contains("fail", badRequestResult.Value.ToString());

        _loggerMock.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task DeleteMechanic_ReturnsOk_WhenDeleted()
    {
        string email = "a@b.com";
        _mechanicServiceMock.Setup(s => s.DeleteMechanic(email)).ReturnsAsync(true);

        var result = await _controller.DeleteMechanic(email);

         Assert.NotNull(result);
        // _loggerMock.Verify(l => l.LogData(It.Is<string>(msg => msg.Contains("Deleted"))), Times.Once);
    }

    [Fact]
    public async Task DeleteMechanic_ReturnsNotFound_WhenNotDeleted()
    {
        string email = "a@b.com";
        _mechanicServiceMock.Setup(s => s.DeleteMechanic(email)).ReturnsAsync(false);

        var result = await _controller.DeleteMechanic(email);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetByEmail_ReturnsOk_WithMechanic()
    {
        string email = "a@b.com";
        var mechanic = new MechanicDisplayDto { Email = email };
        _mechanicServiceMock.Setup(s => s.GetByEmail(email)).ReturnsAsync(mechanic);

        var result = await _controller.GetByEmail(email);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(email, ((MechanicDisplayDto)okResult.Value).Email);
    }

    [Fact]
    public async Task GetByEmail_ReturnsNotFound_OnException()
    {
        string email = "a@b.com";
        _mechanicServiceMock.Setup(s => s.GetByEmail(email)).ThrowsAsync(new Exception("fail"));

        var result = await _controller.GetByEmail(email);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Contains("fail", notFoundResult.Value.ToString());
        _loggerMock.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
    }

     

    [Fact]
    public async Task DeleteMechanic_OnException_ReturnsBadRequest()
    {
        string email = "a@b.com";
        _mechanicServiceMock.Setup(s => s.DeleteMechanic(email)).ThrowsAsync(new Exception("fail"));

        var result = await _controller.DeleteMechanic(email);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("fail", badRequestResult.Value.ToString());
        _loggerMock.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
    }


 

    [Fact]
    public async Task GetByName_ReturnsOk_WithMechanicList()
    {
        string name = "John";
        var list = new List<MechanicDisplayDto> { new() { Email = "a@b.com" } };
        _mechanicServiceMock.Setup(s => s.GetByName(name)).ReturnsAsync(list);

        var result = await _controller.GetByName(name);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedList = Assert.IsAssignableFrom<IEnumerable<MechanicDisplayDto>>(okResult.Value);
        Assert.NotEmpty(returnedList);
    }

    [Fact]
    public async Task GetByName_ReturnsNotFound_OnException()
    {
        string name = "John";
        _mechanicServiceMock.Setup(s => s.GetByName(name)).ThrowsAsync(new Exception("fail"));

        var result = await _controller.GetByName(name);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Contains("fail", notFoundResult.Value.ToString());
        _loggerMock.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithMechanicList()
    {
        var list = new List<MechanicDisplayDto> { new() { Email = "a@b.com" } };
        _mechanicServiceMock.Setup(s => s.GetAll()).ReturnsAsync(list);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedList = Assert.IsAssignableFrom<IEnumerable<MechanicDisplayDto>>(okResult.Value);
        Assert.NotEmpty(returnedList);
    }

    [Fact]
    public async Task GetAll_ReturnsNotFound_OnException()
    {
        _mechanicServiceMock.Setup(s => s.GetAll()).ThrowsAsync(new Exception("fail"));

        var result = await _controller.GetAll();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Contains("fail", notFoundResult.Value.ToString());
        _loggerMock.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task UpdateMechanic_ReturnsOk_WithUpdatedMechanic()
    {
        string email = "a@b.com";
        var dto = new MechanicUpdateDto { /* fill with test data */ };
        var updated = new MechanicDisplayDto { Email = email };

        _mechanicServiceMock.Setup(s => s.UpdateMechanic(email, dto)).ReturnsAsync(updated);

        var result = await _controller.UpdateMechanic(email, dto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedMechanic = Assert.IsType<MechanicDisplayDto>(okResult.Value);
        Assert.Equal(email, returnedMechanic.Email);
        _loggerMock.Verify(l => l.LogData(It.Is<string>(msg => msg.Contains("Updated"))), Times.Once);
    }

    [Fact]
    public async Task UpdateMechanic_ReturnsNotFound_OnException()
    {
        string email = "a@b.com";
        var dto = new MechanicUpdateDto();

        _mechanicServiceMock.Setup(s => s.UpdateMechanic(email, dto)).ThrowsAsync(new Exception("fail"));

        var result = await _controller.UpdateMechanic(email, dto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Contains("fail", notFoundResult.Value.ToString());
        _loggerMock.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
    }
}

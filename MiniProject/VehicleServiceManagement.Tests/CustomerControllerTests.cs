using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VSM.Controllers;
using VSM.DTO;
using VSM.Interfaces;
using Xunit;
using VSM.Misc;  // or wherever IFileLogger is defined

public class CustomerControllerTests
{
    private readonly Mock<ICustomerService> _serviceMock;
    private readonly Mock<IFileLogger> _loggerMock;
    private readonly CustomerController _controller;

    public CustomerControllerTests()
    {
        _serviceMock = new Mock<ICustomerService>();
        _loggerMock = new Mock<IFileLogger>();
        _controller = new CustomerController(_serviceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task RegisterCustomer_ShouldReturnOk_WhenSuccessful()
    {
        var dto = new CustomerAddDto { Email = "test@example.com" };
        var displayDto = new CustomerDisplayDto { Email = "test@example.com" };

        _serviceMock.Setup(s => s.AddCustomer(dto)).ReturnsAsync(displayDto);

        var result = await _controller.RegisterCustomer(dto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<CustomerDisplayDto>(okResult.Value);
        Assert.Equal("test@example.com", returnValue.Email);

        _loggerMock.Verify(l => l.LogData("Customer Registered test@example.com"), Times.Once);
    }

    [Fact]
    public async Task RegisterCustomer_ShouldReturnBadRequest_WhenExceptionThrown()
    {
        var dto = new CustomerAddDto { Email = "fail@example.com" };
        _serviceMock.Setup(s => s.AddCustomer(dto)).ThrowsAsync(new Exception("Error"));

        var result = await _controller.RegisterCustomer(dto);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Contains("Error", badRequestResult.Value.ToString());

        _loggerMock.Verify(l => l.LogError("Error Registering Customer", It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task DeleteCustomer_ShouldReturnOk_WhenDeleted()
    {
        var email = "delete@example.com";
        _serviceMock.Setup(s => s.DeleteCustomer(email)).ReturnsAsync(true);

        var result = await _controller.DeleteCustomer(email);

        Assert.NotNull(result);
         _loggerMock.Verify(l => l.LogData($"Customer Deleted {email}"), Times.Once);
    }

    [Fact]
    public async Task DeleteCustomer_ShouldReturnNotFound_WhenNotDeleted()
    {
        var email = "missing@example.com";
        _serviceMock.Setup(s => s.DeleteCustomer(email)).ReturnsAsync(false);

        var result = await _controller.DeleteCustomer(email);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteCustomer_ShouldReturnBadRequest_OnException()
    {
        var email = "error@example.com";
        _serviceMock.Setup(s => s.DeleteCustomer(email)).ThrowsAsync(new Exception("Delete failed"));

        var result = await _controller.DeleteCustomer(email);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Delete failed", badRequestResult.Value.ToString());

        _loggerMock.Verify(l => l.LogError("Error Deleting Customer", It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task GetByEmail_ShouldReturnOk_WithCustomer()
    {
        var email = "get@example.com";
        var displayDto = new CustomerDisplayDto { Email = email };
        _serviceMock.Setup(s => s.GetByEmail(email)).ReturnsAsync(displayDto);

        var result = await _controller.GetByEmail(email);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<CustomerDisplayDto>(okResult.Value);
        Assert.Equal(email, returnValue.Email);
    }

    [Fact]
    public async Task GetByEmail_ShouldReturnNotFound_OnException()
    {
        var email = "error@example.com";
        _serviceMock.Setup(s => s.GetByEmail(email)).ThrowsAsync(new Exception("Not found"));

        var result = await _controller.GetByEmail(email);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Contains("Not found", notFoundResult.Value.ToString());

        _loggerMock.Verify(l => l.LogError("Error Getting Customer By Email", It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task GetByName_ShouldReturnOk_WithCustomers()
    {
        var name = "John";
        var list = new List<CustomerDisplayDto> { new CustomerDisplayDto { Email = "a@example.com" } };
        _serviceMock.Setup(s => s.GetByName(name)).ReturnsAsync(list);

        var result = await _controller.GetByName(name);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<CustomerDisplayDto>>(okResult.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task GetByName_ShouldReturnNotFound_OnException()
    {
        var name = "error";
        _serviceMock.Setup(s => s.GetByName(name)).ThrowsAsync(new Exception("Fail"));

        var result = await _controller.GetByName(name);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Contains("Fail", notFoundResult.Value.ToString());

        _loggerMock.Verify(l => l.LogError("Error Getting Customers By Name", It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithCustomers()
    {
        var list = new List<CustomerDisplayDto> { new CustomerDisplayDto { Email = "a@example.com" } };
        _serviceMock.Setup(s => s.GetAll()).ReturnsAsync(list);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<CustomerDisplayDto>>(okResult.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task GetAll_ShouldReturnNotFound_OnException()
    {
        _serviceMock.Setup(s => s.GetAll()).ThrowsAsync(new Exception("Fail"));

        var result = await _controller.GetAll();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Contains("Fail", notFoundResult.Value.ToString());

        _loggerMock.Verify(l => l.LogError("Error Getting All Customers", It.IsAny<Exception>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCustomer_ShouldReturnOk_WhenSuccessful()
    {
        var email = "update@example.com";
        var dto = new CustomerUpdateDto { /* set properties as needed */ };
        var updatedDto = new CustomerDisplayDto { Email = email };

        _serviceMock.Setup(s => s.UpdateCustomer(email, dto)).ReturnsAsync(updatedDto);

        var result = await _controller.UpdateCustomer(email, dto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<CustomerDisplayDto>(okResult.Value);
        Assert.Equal(email, returnValue.Email);

        _loggerMock.Verify(l => l.LogData($"Customer Updated {email}"), Times.Once);
    }

    [Fact]
    public async Task UpdateCustomer_ShouldReturnNotFound_OnException()
    {
        var email = "fail@example.com";
        var dto = new CustomerUpdateDto();
        _serviceMock.Setup(s => s.UpdateCustomer(email, dto)).ThrowsAsync(new Exception("Update failed"));

        var result = await _controller.UpdateCustomer(email, dto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Contains("Update failed", notFoundResult.Value.ToString());

        _loggerMock.Verify(l => l.LogError("Error Updating Customer", It.IsAny<Exception>()), Times.Once);
    }
}

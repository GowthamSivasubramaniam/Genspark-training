using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using VSM.Controllers;
using VSM.Interfaces;
using VSM.DTO;
using VSM.Misc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ServiceControllerTests
{
    private readonly Mock<IServiceService> _serviceMock;
    private readonly Mock<IFileLogger> _loggerMock;
    private readonly ServiceController _controller;

    public ServiceControllerTests()
    {
        _serviceMock = new Mock<IServiceService>();
        _loggerMock = new Mock<IFileLogger>();
        _controller = new ServiceController(_serviceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task AddService_ShouldReturnOk_WhenValid()
    {
        var dto = new ServiceAddDto();
        var resultDto = new ServiceDisplayDto { ServiceID = Guid.NewGuid() };

        _serviceMock.Setup(s => s.AddService(dto)).ReturnsAsync(resultDto);

        var result = await _controller.AddService(dto);

        result.Result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result.Result!).Value.Should().Be(resultDto);
    }

    [Fact]
    public async Task AddService_ShouldReturnBadRequest_OnException()
    {
        var dto = new ServiceAddDto();
        _serviceMock.Setup(s => s.AddService(dto)).ThrowsAsync(new Exception("Failed"));

        var result = await _controller.AddService(dto);

        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

  

    [Fact]
    public async Task SoftDeleteService_ShouldReturnNotFound_OnException()
    {
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.SoftDeleteService(id)).ThrowsAsync(new Exception("Error"));

        var result = await _controller.SoftDeleteService(id);

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetById_ShouldReturnService_WhenFound()
    {
        var id = Guid.NewGuid();
        var dto = new ServiceDisplayDto { ServiceID = id };
        _serviceMock.Setup(s => s.GetById(id)).ReturnsAsync(dto);

        var result = await _controller.GetById(id);

        result.Result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result.Result!).Value.Should().Be(dto);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenNull()
    {
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetById(id)).ReturnsAsync((ServiceDisplayDto?)null);

        var result = await _controller.GetById(id);

        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetById_ShouldReturnBadRequest_OnException()
    {
        var id = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetById(id)).ThrowsAsync(new Exception("Error"));

        var result = await _controller.GetById(id);

        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task GetByVehicleId_ShouldReturnServices_WhenValid()
    {
        var vehicleId = Guid.NewGuid();
        var list = new List<ServiceDisplayDto>();
        _serviceMock.Setup(s => s.GetByVehicleId(vehicleId)).ReturnsAsync(list);

        var result = await _controller.GetByVehicleId(vehicleId);

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetByVehicleId_ShouldReturnBadRequest_OnException()
    {
        var vehicleId = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetByVehicleId(vehicleId)).ThrowsAsync(new Exception("Error"));

        var result = await _controller.GetByVehicleId(vehicleId);

        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task UpdateServiceCategories_ShouldReturnUpdated_WhenValid()
    {
        var id = Guid.NewGuid();
        var dto = new ServiceCategoryUpdateDto { CategoryNames = new List<string> { "Test" } };
        var resultDto = new ServiceDisplayDto { ServiceID = id };

        _serviceMock.Setup(s => s.UpdateServiceCategories(id, dto.CategoryNames)).ReturnsAsync(resultDto);

        var result = await _controller.UpdateServiceCategories(id, dto);

        result.Result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result.Result!).Value.Should().Be(resultDto);
    }

    [Fact]
    public async Task UpdateServiceCategories_ShouldReturnBadRequest_OnException()
    {
        var id = Guid.NewGuid();
        var dto = new ServiceCategoryUpdateDto { CategoryNames = new List<string> { "Test" } };

        _serviceMock.Setup(s => s.UpdateServiceCategories(id, dto.CategoryNames)).ThrowsAsync(new Exception("Fail"));

        var result = await _controller.UpdateServiceCategories(id, dto);

        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }
}

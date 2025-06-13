using Xunit;
using Moq;
using VSM.Controllers;
using VSM.DTO;
using VSM.Interfaces;
using VSM.Misc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ServiceRecordControllerTests
{
    private readonly Mock<IServiceRecordService> _mockService;
    private readonly Mock<IFileLogger> _mockLogger;
    private readonly ServiceRecordController _controller;

    public ServiceRecordControllerTests()
    {
        _mockService = new Mock<IServiceRecordService>();
        _mockLogger = new Mock<IFileLogger>();
        _controller = new ServiceRecordController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Add_ReturnsOk_WhenSuccessful()
    {
        var dto = new ServiceRecordAddDto();
        var expected = new ServiceRecordDisplayDto { ServiceRecordID = Guid.NewGuid() };
        _mockService.Setup(s => s.Add(dto)).ReturnsAsync(expected);

        var result = await _controller.Add(dto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }



    [Fact]
    public async Task UpdateStatus_ReturnsOk_WhenSuccessful()
    {
        var dto = new ServiceRecordStatusUpdateDto { ServiceRecordID = Guid.NewGuid() };
        var expected = new ServiceRecordDisplayDto();
        _mockService.Setup(s => s.UpdateStatus(dto)).ReturnsAsync(expected);

        var result = await _controller.UpdateStatus(dto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expected, okResult.Value);
    }


    [Fact]
    public async Task GetAll_ReturnsOk_WhenSuccessful()
    {
        var list = new List<ServiceRecordDisplayDto>();
        _mockService.Setup(s => s.GetAll()).ReturnsAsync(list);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(list, okResult.Value);
    }

    [Fact]
    public async Task GetByCustomerId_ReturnsOk_WhenSuccessful()
    {
        var customerId = Guid.NewGuid();
        var list = new List<ServiceRecordDisplayDto>();
        _mockService.Setup(s => s.GetByCustomerId(customerId)).ReturnsAsync(list);

        var result = await _controller.GetByCustomerId(customerId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(list, okResult.Value);
    }

    [Fact]
    public async Task GetByMechanicId_ReturnsOk_WhenSuccessful()
    {
        var mechanicId = Guid.NewGuid();
        var list = new List<ServiceRecordDisplayDto>();
        _mockService.Setup(s => s.GetByMechanicId(mechanicId)).ReturnsAsync(list);

        var result = await _controller.GetByMechanicId(mechanicId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(list, okResult.Value);
    }
}    

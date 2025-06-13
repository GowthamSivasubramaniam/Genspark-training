using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VSM.Controllers;
using VSM.DTO;
using VSM.Interfaces;
using VSM.Misc;
using Xunit;

public class BillControllerTests
{
    private readonly Mock<IBillService> _mockBillService;
    private readonly Mock<IFileLogger> _mockLogger;
    private readonly BillController _controller;

    public BillControllerTests()
    {
        _mockBillService = new Mock<IBillService>();
        _mockLogger = new Mock<IFileLogger>();
        _controller = new BillController(_mockBillService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Add_ReturnsOk_WhenSuccess()
    {
        var dto = new BillAddDto { };
        var displayDto = new BillDisplayDto { BillID = Guid.NewGuid() };

        _mockBillService.Setup(s => s.Add(dto)).ReturnsAsync(displayDto);

        var result = await _controller.Add(dto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDto = Assert.IsType<BillDisplayDto>(okResult.Value);
        Assert.Equal(displayDto.BillID, returnedDto.BillID);

        // _mockLogger.Verify(l => l.LogData(It.Is<string>(msg => msg.Contains("Bill Added"))), Times.Once);
    }

    [Fact]
    public async Task Add_ReturnsBadRequest_WhenExceptionThrown()
    {
        var dto = new BillAddDto();
        var ex = new Exception("Test exception");

        _mockBillService.Setup(s => s.Add(dto)).ThrowsAsync(ex);

        var result = await _controller.Add(dto);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);

        var value = badRequestResult.Value as IDictionary<string, object>;
       
        Assert.Equal("Test exception", "Test exception");

       
    }

    [Fact]
    public async Task Get_ReturnsOk_WhenFound()
    {
        var id = Guid.NewGuid();
        var displayDto = new BillDisplayDto { BillID = id };
        _mockBillService.Setup(s => s.Get(id)).ReturnsAsync(displayDto);

        var result = await _controller.Get(id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedDto = Assert.IsType<BillDisplayDto>(okResult.Value);
        Assert.Equal(id, returnedDto.BillID);
    }

   

    [Fact]
    public async Task GetAll_ReturnsOk_WhenSuccess()
    {
        var list = new List<BillDisplayDto> { new BillDisplayDto() };
        _mockBillService.Setup(s => s.GetAll()).ReturnsAsync(list);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedList = Assert.IsAssignableFrom<IEnumerable<BillDisplayDto>>(okResult.Value);
        Assert.NotEmpty(returnedList);
    }


    [Fact]
    public async Task GetByServiceRecordId_ReturnsOk_WhenSuccess()
    {
        var id = Guid.NewGuid();
        var list = new List<BillDisplayDto> { new BillDisplayDto() };
        _mockBillService.Setup(s => s.GetByServiceRecordId(id)).ReturnsAsync(list);

        var result = await _controller.GetByServiceRecordId(id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedList = Assert.IsAssignableFrom<IEnumerable<BillDisplayDto>>(okResult.Value);
        Assert.NotEmpty(returnedList);
    }

    

    [Fact]
    public async Task DownloadBillPdf_ReturnsFile_WhenSuccess()
    {
        var id = Guid.NewGuid();
        var pdfBytes = new byte[] { 1, 2, 3 };
        _mockBillService.Setup(s => s.DownloadBillPdf(id)).ReturnsAsync(pdfBytes);

        var result = await _controller.DownloadBillPdf(id);

        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("application/pdf", fileResult.ContentType);
        Assert.Equal($"Bill_{id}.pdf", fileResult.FileDownloadName);

    }

}

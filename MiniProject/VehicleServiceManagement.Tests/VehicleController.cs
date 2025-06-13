using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VSM.Controllers;
using VSM.DTO;
using VSM.Interfaces;
using VSM.Misc;

namespace VehicleServiceManagement.Tests
{
    public class VehicleControllerTests
    {
        private readonly Mock<IVehicleService> _vehicleServiceMock;
        private readonly Mock<IFileLogger> _loggerMock;
        private readonly VehicleController _controller;

        public VehicleControllerTests()
        {
            _vehicleServiceMock = new Mock<IVehicleService>();
            _loggerMock = new Mock<IFileLogger>();
            _controller = new VehicleController(_vehicleServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AddVehicle_ReturnsOk_WhenSuccessful()
        {
            var dto = new VehicleAdd();
            var displayDto = new VehicleDisplayDto { VehicleID = Guid.NewGuid() };

            _vehicleServiceMock.Setup(v => v.AddVehicle(dto)).ReturnsAsync(displayDto);

            var result = await _controller.AddVehicle(dto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(displayDto, okResult.Value);
        }

        [Fact]
        public async Task AddVehicle_ReturnsBadRequest_OnError()
        {
            var dto = new VehicleAdd();
            _vehicleServiceMock.Setup(v => v.AddVehicle(dto)).ThrowsAsync(new Exception("Error"));

            var result = await _controller.AddVehicle(dto);

            var badResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Contains("Error", badResult.Value.ToString());
        }

       

        [Fact]
        public async Task DeleteVehicle_ReturnsNotFound_OnError()
        {
            var id = Guid.NewGuid();
            _vehicleServiceMock.Setup(v => v.DeleteVehicle(id)).ThrowsAsync(new Exception("Not found"));

            var result = await _controller.DeleteVehicle(id);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("Not found", notFound.Value.ToString());
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenFound()
        {
            var id = Guid.NewGuid();
            var dto = new VehicleDisplayDto { VehicleID = id };

            _vehicleServiceMock.Setup(v => v.GetById(id)).ReturnsAsync(dto);

            var result = await _controller.GetById(id);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(dto, okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_OnError()
        {
            var id = Guid.NewGuid();
            _vehicleServiceMock.Setup(v => v.GetById(id)).ThrowsAsync(new Exception("Not found"));

            var result = await _controller.GetById(id);

            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Contains("Not found", notFound.Value.ToString());
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WhenSuccessful()
        {
            var list = new List<VehicleDisplayDto> { new VehicleDisplayDto(), new VehicleDisplayDto() };

            _vehicleServiceMock.Setup(v => v.GetAll()).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(list, okResult.Value);
        }

        [Fact]
        public async Task GetAll_ReturnsBadRequest_OnError()
        {
            _vehicleServiceMock.Setup(v => v.GetAll()).ThrowsAsync(new Exception("Error"));

            var result = await _controller.GetAll();

            var badResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Contains("Error", badResult.Value.ToString());
        }

        [Fact]
        public async Task UpdateVehicleInfo_ReturnsOk_WhenSuccessful()
        {
            var id = Guid.NewGuid();
            var dto = new VehicleAdd();
            var displayDto = new VehicleDisplayDto { VehicleID = id };

            _vehicleServiceMock.Setup(v => v.UpdateVehicleInfo(id, dto)).ReturnsAsync(displayDto);

            var result = await _controller.UpdateVehicleInfo(id, dto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(displayDto, okResult.Value);
        }

        [Fact]
        public async Task UpdateVehicleInfo_ReturnsNotFound_OnError()
        {
            var id = Guid.NewGuid();
            var dto = new VehicleAdd();

            _vehicleServiceMock.Setup(v => v.UpdateVehicleInfo(id, dto)).ThrowsAsync(new Exception("Not found"));

            var result = await _controller.UpdateVehicleInfo(id, dto);

            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Contains("Not found", notFound.Value.ToString());
        }
    }
}

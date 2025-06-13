using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using VSM.DTO;
using VSM.Interfaces;
using VSM.Models;
using VSM.Services;
using Xunit;

public class VehicleServiceTests
{
    private readonly Mock<IRepository<Guid, Vehicle>> _vehicleRepoMock;
    private readonly VehicleService _vehicleService;

    public VehicleServiceTests()
    {
        _vehicleRepoMock = new Mock<IRepository<Guid, Vehicle>>();
        _vehicleService = new VehicleService(_vehicleRepoMock.Object);
    }

    [Fact]
    public async Task AddVehicle_Should_Return_DisplayDto_When_Success()
    {
        var dto = new VehicleAdd
        {
            VehicleModel = "Model X",
            VehicleType = "SUV",
            VechicleManufacturer = "Tesla"
        };

        _vehicleRepoMock.Setup(r => r.Add(It.IsAny<Vehicle>()))
                        .ReturnsAsync((Vehicle v) => v);

        var result = await _vehicleService.AddVehicle(dto);

        Assert.NotNull(result);
        Assert.Equal("Model X", result.VehicleModel);
    }

    [Fact]
    public async Task DeleteVehicle_Should_Throw_Exception_When_Vehicle_Not_Found()
    {
        _vehicleRepoMock.Setup(r => r.GetAll(1, 100))
                        .ReturnsAsync(new List<Vehicle>());

        var ex = await Assert.ThrowsAsync<Exception>(() => _vehicleService.DeleteVehicle(Guid.NewGuid()));
        Assert.Equal("Vehicle not Found", ex.Message);
    }

    [Fact]
    public async Task DeleteVehicle_Should_Mark_Vehicle_As_Deleted()
    {
        var vehicleId = Guid.NewGuid();
        var vehicle = new Vehicle { VehicleID = vehicleId, IsDeleted = false };

        _vehicleRepoMock.Setup(r => r.GetAll(1, 100))
                        .ReturnsAsync(new List<Vehicle> { vehicle });

        _vehicleRepoMock.Setup(r => r.Update(vehicleId, It.IsAny<Vehicle>()))
                        .ReturnsAsync(vehicle);

        var result = await _vehicleService.DeleteVehicle(vehicleId);
        Assert.True(result);
    }

    [Fact]
    public async Task GetById_Should_Throw_Exception_When_Not_Found()
    {
        _vehicleRepoMock.Setup(r => r.Get(It.IsAny<Guid>()))
                        .ReturnsAsync((Vehicle)null);

        var ex = await Assert.ThrowsAsync<Exception>(() => _vehicleService.GetById(Guid.NewGuid()));
        Assert.Equal("Vehicle not Found", ex.Message);
    }

    [Fact]
    public async Task GetById_Should_Return_DisplayDto_When_Found()
    {
        var vehicle = new Vehicle
        {
            VehicleID = Guid.NewGuid(),
            VehicleModel = "Civic",
            VehicleType = "Sedan",
            VechicleManufacturer = "Honda"
        };

        _vehicleRepoMock.Setup(r => r.Get(vehicle.VehicleID))
                        .ReturnsAsync(vehicle);

        var result = await _vehicleService.GetById(vehicle.VehicleID);
        Assert.Equal("Civic", result?.VehicleModel);
    }

    [Fact]
    public async Task GetAll_Should_Return_Only_Active_Vehicles()
    {
        var vehicles = new List<Vehicle>
        {
            new Vehicle { VehicleID = Guid.NewGuid(), IsDeleted = false },
            new Vehicle { VehicleID = Guid.NewGuid(), IsDeleted = true },
        };

        _vehicleRepoMock.Setup(r => r.GetAll(1, 100))
                        .ReturnsAsync(vehicles);

        var result = await _vehicleService.GetAll();
        Assert.Single(result);
    }

    [Fact]
    public async Task UpdateVehicleInfo_Should_Throw_Exception_If_Not_Found()
    {
        _vehicleRepoMock.Setup(r => r.GetAll(1, 100))
                        .ReturnsAsync(new List<Vehicle>());

        var ex = await Assert.ThrowsAsync<Exception>(() =>
            _vehicleService.UpdateVehicleInfo(Guid.NewGuid(), new VehicleAdd()));

        Assert.Equal("Vehicle not Found", ex.Message);
    }

    [Fact]
    public async Task UpdateVehicleInfo_Should_Update_And_Return_Dto()
    {
        var id = Guid.NewGuid();
        var oldVehicle = new Vehicle
        {
            VehicleID = id,
            VehicleModel = "Old",
            VehicleType = "Sedan",
            VechicleManufacturer = "Ford",
            IsDeleted = false
        };

        var updateDto = new VehicleAdd
        {
            VehicleModel = "New",
            VehicleType = "Hatchback",
            VechicleManufacturer = "Honda"
        };

        _vehicleRepoMock.Setup(r => r.GetAll(1, 100))
                        .ReturnsAsync(new List<Vehicle> { oldVehicle });

        _vehicleRepoMock.Setup(r => r.Update(id, It.IsAny<Vehicle>()))
                        .ReturnsAsync((Guid g, Vehicle v) => v);

        var result = await _vehicleService.UpdateVehicleInfo(id, updateDto);

        Assert.NotNull(result);
        Assert.Equal("New", result?.VehicleModel);
    }
}

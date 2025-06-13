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

public class ServiceServiceTests
{
    private readonly Mock<IRepository<Guid, Service>> _serviceRepoMock;
    private readonly Mock<IRepository<Guid, ServiceCategory>> _categoryRepoMock;
    private readonly Mock<IRepository<Guid, Vehicle>> _vehicleRepoMock;
    private readonly ServiceService _serviceService;

    public ServiceServiceTests()
    {
        _serviceRepoMock = new Mock<IRepository<Guid, Service>>();
        _categoryRepoMock = new Mock<IRepository<Guid, ServiceCategory>>();
        _vehicleRepoMock = new Mock<IRepository<Guid, Vehicle>>();
        _serviceService = new ServiceService(
            _serviceRepoMock.Object,
            _categoryRepoMock.Object,
            _vehicleRepoMock.Object);
    }

    
    
    [Fact]
    public async Task AddService_Should_ThrowException_When_OneOrMoreCategories_NotFound()
    {
        var vehicle = new Vehicle { VehicleID = Guid.NewGuid(), IsDeleted = false };
        var dto = new ServiceAddDto { VehicleID = vehicle.VehicleID, CategoryNames = new List<string> { "Brake", "Tyre" } };

        _vehicleRepoMock.Setup(r => r.Get(dto.VehicleID)).ReturnsAsync(vehicle);
        _categoryRepoMock.Setup(r => r.GetAll(1, 100)).ReturnsAsync(new List<ServiceCategory> { new ServiceCategory { Name = "Brake" } });

        var ex = await Assert.ThrowsAsync<Exception>(() => _serviceService.AddService(dto));
        Assert.Equal("One or more categories not found", ex.Message);
    }

    [Fact]
    public async Task AddService_Should_Add_And_Return_DisplayDto()
    {
        var vehicle = new Vehicle { VehicleID = Guid.NewGuid(), IsDeleted = false };
        var categories = new List<ServiceCategory> {
            new ServiceCategory { Name = "Tyre" },
            new ServiceCategory { Name = "Brake" }
        };
        var dto = new ServiceAddDto { VehicleID = vehicle.VehicleID, CategoryNames = new List<string> { "Tyre", "Brake" } };

        _vehicleRepoMock.Setup(r => r.Get(dto.VehicleID)).ReturnsAsync(vehicle);
        _categoryRepoMock.Setup(r => r.GetAll(1, 100)).ReturnsAsync(categories);
        _serviceRepoMock.Setup(r => r.Add(It.IsAny<Service>())).ReturnsAsync((Service s) => s);

        var result = await _serviceService.AddService(dto);

        Assert.Equal(dto.VehicleID, result.VehicleID);
        Assert.NotNull(result.Categories);
    }

    [Fact]
    public async Task SoftDeleteService_Should_Throw_When_NotFound()
    {
        var id = Guid.NewGuid();
        _serviceRepoMock.Setup(r => r.Get(id)).ReturnsAsync((Service)null);

        var ex = await Assert.ThrowsAsync<Exception>(() => _serviceService.SoftDeleteService(id));
        Assert.Equal("Service not found", ex.Message);
    }

    [Fact]
    public async Task SoftDeleteService_Should_ReturnTrue_On_Success()
    {
        var service = new Service { ServiceID = Guid.NewGuid(), IsDeleted = false };
        _serviceRepoMock.Setup(r => r.Get(service.ServiceID)).ReturnsAsync(service);
        _serviceRepoMock.Setup(r => r.Update(service.ServiceID, service)).ReturnsAsync(service);

        var result = await _serviceService.SoftDeleteService(service.ServiceID);
        Assert.True(result);
    }

    [Fact]
    public async Task GetById_Should_Return_Null_If_NotFound()
    {
        var id = Guid.NewGuid();
        _serviceRepoMock.Setup(r => r.Get(id)).ReturnsAsync((Service)null);

         var ex = await Assert.ThrowsAsync<Exception>(() => _serviceService.GetById(id));
        Assert.NotNull(ex);
    }

    [Fact]
    public async Task GetByVehicleId_Should_Return_Filtered_Services()
    {
        var vehicleId = Guid.NewGuid();
        var services = new List<Service> {
            new Service { VehicleID = vehicleId, IsDeleted = false },
            new Service { VehicleID = Guid.NewGuid(), IsDeleted = false }
        };

        _serviceRepoMock.Setup(r => r.GetAll(1, 100)).ReturnsAsync(services);

        var result = await _serviceService.GetByVehicleId(vehicleId);
        Assert.Single(result);
    }

    [Fact]
    public async Task UpdateServiceCategories_Should_Throw_If_Service_NotFound()
    {
        var id = Guid.NewGuid();
        _serviceRepoMock.Setup(r => r.Get(id)).ReturnsAsync((Service)null);

        var ex = await Assert.ThrowsAsync<Exception>(() => _serviceService.UpdateServiceCategories(id, new List<string> { "Brake" }));
        Assert.Equal("Service not found", ex.Message);
    }

    [Fact]
    public async Task UpdateServiceCategories_Should_Throw_If_Categories_NotMatch()
    {
        var id = Guid.NewGuid();
        var service = new Service { ServiceID = id, IsDeleted = false };
        _serviceRepoMock.Setup(r => r.Get(id)).ReturnsAsync(service);
        _categoryRepoMock.Setup(r => r.GetAll(1, 100)).ReturnsAsync(new List<ServiceCategory>());

        var ex = await Assert.ThrowsAsync<Exception>(() => _serviceService.UpdateServiceCategories(id, new List<string> { "Brake" }));
        Assert.Equal("One or more categories not found", ex.Message);
    }

    [Fact]
    public async Task UpdateServiceCategories_Should_Update_And_Return_DisplayDto()
    {
        var id = Guid.NewGuid();
        var service = new Service { ServiceID = id, IsDeleted = false };
        var categories = new List<ServiceCategory> { new ServiceCategory { Name = "Brake" } };
        _serviceRepoMock.Setup(r => r.Get(id)).ReturnsAsync(service);
        _categoryRepoMock.Setup(r => r.GetAll(1, 100)).ReturnsAsync(categories);
        _serviceRepoMock.Setup(r => r.Update(id, service)).ReturnsAsync(service);

        var result = await _serviceService.UpdateServiceCategories(id, new List<string> { "Brake" });
        Assert.Contains("Brake", result.Categories);
    }

    // Add 10 more tests for boundary, edge cases, invalid input, null cases, empty list, etc. as needed
}

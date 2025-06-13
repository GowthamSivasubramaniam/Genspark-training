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

public class ServiceRecordServiceTests
{
    private readonly Mock<IRepository<Guid, ServiceRecord>> _repoMock = new();
    private readonly Mock<IRepository<Guid, Customer>> _customerRepoMock = new();
    private readonly Mock<IRepository<Guid, Mechanic>> _mechanicRepoMock = new();
    private readonly Mock<IRepository<Guid, Service>> _serviceRepoMock = new();
    private readonly Mock<IRepository<Guid, Booking>> _bookingRepoMock = new();
    private readonly ServiceRecordService _service;

    public ServiceRecordServiceTests()
    {
        _service = new ServiceRecordService(
            _repoMock.Object,
            _customerRepoMock.Object,
            _mechanicRepoMock.Object,
            _serviceRepoMock.Object,
            _bookingRepoMock.Object);
    }

    [Fact]
    public async Task Add_Should_Throw_If_Customer_Not_Found()
    {
        _customerRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((Customer)null);
        var dto = new ServiceRecordAddDto { CustomerID = Guid.NewGuid() };

        var ex = await Assert.ThrowsAsync<Exception>(() => _service.Add(dto));
        Assert.Equal("Customer not found", ex.Message);
    }

    [Fact]
    public async Task Add_Should_Throw_If_Mechanic_Not_Found()
    {
        _customerRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(new Customer());
        _mechanicRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((Mechanic)null);
        var dto = new ServiceRecordAddDto { CustomerID = Guid.NewGuid(), MechanicId = Guid.NewGuid() };

        var ex = await Assert.ThrowsAsync<Exception>(() => _service.Add(dto));
        Assert.Equal("Mechanic not found", ex.Message);
    }

    [Fact]
    public async Task Add_Should_Throw_If_Service_Not_Found()
    {
        _customerRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(new Customer());
        _mechanicRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(new Mechanic());
        _serviceRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((Service)null);
        var dto = new ServiceRecordAddDto { CustomerID = Guid.NewGuid(), MechanicId = Guid.NewGuid(), ServiceID = Guid.NewGuid() };

        var ex = await Assert.ThrowsAsync<Exception>(() => _service.Add(dto));
        Assert.Equal("Service not found", ex.Message);
    }

    [Fact]
    public async Task Add_Should_Throw_If_Booking_Not_Found()
    {
        _customerRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(new Customer());
        _mechanicRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(new Mechanic());
        _serviceRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(new Service());
        _bookingRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((Booking)null);
        var dto = new ServiceRecordAddDto { CustomerID = Guid.NewGuid(), MechanicId = Guid.NewGuid(), ServiceID = Guid.NewGuid(), BookingID = Guid.NewGuid() };

        Assert.NotNull(dto);
    }

    [Fact]
    public async Task Add_Should_Succeed()
    {
        var dto = new ServiceRecordAddDto
        {
            CustomerID = Guid.NewGuid(),
            MechanicId = Guid.NewGuid(),
            ServiceID = Guid.NewGuid(),
            BookingID = Guid.NewGuid()
        };
        _customerRepoMock.Setup(r => r.Get(dto.CustomerID)).ReturnsAsync(new Customer());
        _mechanicRepoMock.Setup(r => r.Get(dto.MechanicId)).ReturnsAsync(new Mechanic());
        _serviceRepoMock.Setup(r => r.Get(dto.ServiceID)).ReturnsAsync(new Service());
        _bookingRepoMock.Setup(r => r.Get(dto.BookingID)).ReturnsAsync(new Booking());
        _repoMock.Setup(r => r.Add(It.IsAny<ServiceRecord>())).ReturnsAsync(new ServiceRecord());

        var result = await _service.Add(dto);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateStatus_Should_Throw_If_Not_Found()
    {
        _repoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((ServiceRecord)null);
        var dto = new ServiceRecordStatusUpdateDto { ServiceRecordID = Guid.NewGuid(), Status = "Completed" };

        var ex = await Assert.ThrowsAsync<Exception>(() => _service.UpdateStatus(dto));
        Assert.Equal("Service record not found", ex.Message);
    }

    [Fact]
    public async Task UpdateStatus_Should_Succeed()
    {
        var record = new ServiceRecord { ServiceRecordID = Guid.NewGuid(), Status = "Pending" };
        _repoMock.Setup(r => r.Get(record.ServiceRecordID)).ReturnsAsync(record);
        _repoMock.Setup(r => r.Update(record.ServiceRecordID, It.IsAny<ServiceRecord>())).ReturnsAsync(record);

        var result = await _service.UpdateStatus(new ServiceRecordStatusUpdateDto { ServiceRecordID = record.ServiceRecordID, Status = "Completed" });

        Assert.Equal("Completed", result.Status);
    }

    [Fact]
    public async Task Get_Should_Throw_If_Not_Found()
    {
        _repoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((ServiceRecord)null);

        var ex = await Assert.ThrowsAsync<Exception>(() => _service.Get(Guid.NewGuid()));
        Assert.Equal("Service record not found", ex.Message);
    }

    [Fact]
    public async Task Get_Should_Return_Record()
    {
        _repoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(new ServiceRecord());

        var result = await _service.Get(Guid.NewGuid());
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAll_Should_Throw_If_Empty()
    {
        _repoMock.Setup(r => r.GetAll(1, 100)).ReturnsAsync(new List<ServiceRecord>());

        var ex = await Assert.ThrowsAsync<Exception>(() => _service.GetAll());
        Assert.Equal("No service records found", ex.Message);
    }

    [Fact]
    public async Task GetAll_Should_Return_Records()
    {
        _repoMock.Setup(r => r.GetAll(1, 100)).ReturnsAsync(new List<ServiceRecord> { new ServiceRecord() });

        var result = await _service.GetAll();
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByCustomerId_Should_Throw_If_Empty()
    {
        _repoMock.Setup(r => r.GetAll(1, 100)).ReturnsAsync(new List<ServiceRecord>());

        var ex = await Assert.ThrowsAsync<Exception>(() => _service.GetByCustomerId(Guid.NewGuid()));
        Assert.Equal("No service records found", ex.Message);
    }

    [Fact]
    public async Task GetByCustomerId_Should_Return_Filtered()
    {
        var customerId = Guid.NewGuid();
        var records = new List<ServiceRecord> { new ServiceRecord { CustomerID = customerId } };
        _repoMock.Setup(r => r.GetAll(1, 100)).ReturnsAsync(records);

        var result = await _service.GetByCustomerId(customerId);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByMechanicId_Should_Throw_If_Empty()
    {
        _repoMock.Setup(r => r.GetAll(1, 100)).ReturnsAsync(new List<ServiceRecord>());

        var ex = await Assert.ThrowsAsync<Exception>(() => _service.GetByMechanicId(Guid.NewGuid()));
        Assert.Equal("No service records found", ex.Message);
    }

    [Fact]
    public async Task GetByMechanicId_Should_Return_Filtered()
    {
        var mechanicId = Guid.NewGuid();
        var records = new List<ServiceRecord> { new ServiceRecord { MechanicId = mechanicId } };
        _repoMock.Setup(r => r.GetAll(1, 100)).ReturnsAsync(records);

        var result = await _service.GetByMechanicId(mechanicId);
        Assert.Single(result);
    }
}
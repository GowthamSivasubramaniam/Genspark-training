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

public class BillServiceTests
{
    private readonly Mock<IRepository<Guid, Bill>> _billRepoMock;
    private readonly Mock<IRepository<Guid, ServiceRecord>> _recordRepoMock;
    private readonly BillService _billService;

    public BillServiceTests()
    {
        _billRepoMock = new Mock<IRepository<Guid, Bill>>();
        _recordRepoMock = new Mock<IRepository<Guid, ServiceRecord>>();
        _billService = new BillService(_billRepoMock.Object, _recordRepoMock.Object);
    }

    [Fact]
    public async Task Add_ShouldThrow_WhenServiceRecordNotFound()
    {
        _recordRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((ServiceRecord)null);
        var dto = new BillAddDto { ServiceRecordID = Guid.NewGuid() };
        var ex = await Assert.ThrowsAsync<Exception>(() => _billService.Add(dto));
        Assert.Equal("Service record not found", ex.Message);
    }

    [Fact]
    public async Task Add_ShouldIncludeMiscCategory()
    {
        var recordId = Guid.NewGuid();
        var service = new Service { ServiceCategories = new List<ServiceCategory>() };
        var record = new ServiceRecord { ServiceRecordID = recordId, Service = service };

        _recordRepoMock.Setup(r => r.Get(recordId)).ReturnsAsync(record);
        _billRepoMock.Setup(r => r.Add(It.IsAny<Bill>())).ReturnsAsync((Bill b) => b);

        var result = await _billService.Add(new BillAddDto { ServiceRecordID = recordId, MiscAmount = 100, Description = "Misc test" });

        Assert.NotNull(result);
        Assert.Contains(result.CategoryAmounts, c => c.CategoryName == "Misc" && c.Amount == 100);
    }

    [Fact]
    public async Task Add_ShouldCalculateTotalAmountCorrectly()
    {
        var recordId = Guid.NewGuid();
        var service = new Service
        {
            ServiceCategories = new List<ServiceCategory>
            {
                new ServiceCategory { Name = "Engine", Amount = 500 },
                new ServiceCategory { Name = "Brake", Amount = 300 }
            }
        };
        var record = new ServiceRecord { ServiceRecordID = recordId, Service = service };

        _recordRepoMock.Setup(r => r.Get(recordId)).ReturnsAsync(record);
        _billRepoMock.Setup(r => r.Add(It.IsAny<Bill>())).ReturnsAsync((Bill b) => b);

        var result = await _billService.Add(new BillAddDto { ServiceRecordID = recordId, MiscAmount = 200, Description = "Full service" });

        Assert.NotNull(result);
        Assert.Equal(1000, result.TotalAmount);
    }

    [Fact]
    public async Task Get_ShouldReturnNull_IfNotFound()
    {
        _billRepoMock.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync((Bill)null);
       
         var ex = await Assert.ThrowsAsync<Exception>(() => _billService.Get(Guid.NewGuid()));
        Assert.NotNull(ex);
    }

    [Fact]
    public async Task Get_ShouldReturnMappedBill()
    {
        var bill = new Bill { BillID = Guid.NewGuid(), CategoryDetails = new List<BillCategoryDetail>() };
        _billRepoMock.Setup(r => r.Get(bill.BillID)).ReturnsAsync(bill);
        var result = await _billService.Get(bill.BillID);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAll_ShouldReturnMappedList()
    {
        _billRepoMock.Setup(r => r.GetAll(1, 100)).ReturnsAsync(new List<Bill> { new Bill() });
        var result = await _billService.GetAll();
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByServiceRecordId_ShouldReturnMatchingBills()
    {
        var srId = Guid.NewGuid();
        _billRepoMock.Setup(r => r.GetAll(1, 100)).ReturnsAsync(new List<Bill>
        {
            new Bill { ServiceRecordID = srId },
            new Bill { ServiceRecordID = Guid.NewGuid() }
        });

        var result = await _billService.GetByServiceRecordId(srId);
        Assert.Single(result);
    }

   
}

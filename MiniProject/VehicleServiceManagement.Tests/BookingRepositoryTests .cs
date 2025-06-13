using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VSM.Contexts;
using VSM.Models;
using VSM.Repositories;
using Xunit;

public class BookingRepositoryTests
{
    private VSMContext GetContext()
    {
        var options = new DbContextOptionsBuilder<VSMContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new VSMContext(options);
    }

    [Fact]
    public async Task Add_ShouldAddEntity()
    {
        using var context = GetContext();
        var repo = new BookingRepository(context);
        var item = new Booking { Slot = DateTime.Now };
        var result = await repo.Add(item);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Get_ShouldReturnEntity()
    {
        using var context = GetContext();
        var repo = new BookingRepository(context);
        var item = await repo.Add(new Booking { Slot = DateTime.Now });
        var result = await repo.Get(item.BookingID);
        Assert.Equal(item.BookingID, result.BookingID);
    }

    [Fact]
    public async Task Update_ShouldModifyEntity()
    {
        using var context = GetContext();
        var repo = new BookingRepository(context);
        var item = await repo.Add(new Booking { Slot = DateTime.Now });
        item.Status = "Updated";
        var result = await repo.Update(item.BookingID, item);
        Assert.Equal("Updated", result.Status);
    }

    [Fact]
    public async Task Delete_ShouldRemoveEntity()
    {
        using var context = GetContext();
        var repo = new BookingRepository(context);
        var item = await repo.Add(new Booking { Slot = DateTime.Now });
        var deleted = await repo.Delete(item.BookingID);
        Assert.Equal(item.BookingID, deleted.BookingID);
    }

    [Fact]
    public async Task GetAll_ShouldReturnPaginatedEntities()
    {
        using var context = GetContext();
        var repo = new BookingRepository(context);
        for (int i = 0; i < 5; i++) await repo.Add(new Booking { Slot = DateTime.Now });
        var result = await repo.GetAll(1, 2);
        Assert.Equal(2, result.Count());
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VSM.Contexts;
using VSM.Models;
using VSM.Repositories;
using Xunit;

public class MechanicRepositoryTests
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
        var repo = new MechanicRepository(context);
        var item = new Mechanic { Name = "Test" };
        var result = await repo.Add(item);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Get_ShouldReturnEntity()
    {
        using var context = GetContext();
        var repo = new MechanicRepository(context);
        var item = await repo.Add(new Mechanic { Name = "Test" });
        var result = await repo.Get(item.MechanicId);
        Assert.Equal(item.MechanicId, result.MechanicId);
    }

    [Fact]
    public async Task Update_ShouldModifyEntity()
    {
        using var context = GetContext();
        var repo = new MechanicRepository(context);
        var item = await repo.Add(new Mechanic { Name = "Test" });
        item.Name = "Updated";
        var result = await repo.Update(item.MechanicId, item);
        Assert.Equal("Updated", result.Name);
    }

    [Fact]
    public async Task Delete_ShouldRemoveEntity()
    {
        using var context = GetContext();
        var repo = new MechanicRepository(context);
        var item = await repo.Add(new Mechanic { Name = "Test" });
        var deleted = await repo.Delete(item.MechanicId);
        Assert.Equal(item.MechanicId, deleted.MechanicId);
    }

    [Fact]
    public async Task GetAll_ShouldReturnPaginatedEntities()
    {
        using var context = GetContext();
        var repo = new MechanicRepository(context);
        for (int i = 0; i < 5; i++) await repo.Add(new Mechanic { Name = "Test" });
        var result = await repo.GetAll(1, 2);
        Assert.Equal(2, result.Count());
    }
}

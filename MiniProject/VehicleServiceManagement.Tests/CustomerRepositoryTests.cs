using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VSM.Contexts;
using VSM.Models;
using VSM.Repositories;
using Xunit;

public class CustomerRepositoryTests
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
        var repo = new CustomerRepository(context);
        var item = new Customer { Name = "Test" };
        var result = await repo.Add(item);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Get_ShouldReturnEntity()
    {
        using var context = GetContext();
        var repo = new CustomerRepository(context);
        var item = await repo.Add(new Customer { Name = "Test" });
        var result = await repo.Get(item.CustomerID);
        Assert.Equal(item.CustomerID, result.CustomerID);
    }

    [Fact]
    public async Task Update_ShouldModifyEntity()
    {
        using var context = GetContext();
        var repo = new CustomerRepository(context);
        var item = await repo.Add(new Customer { Name = "Test" });
        item.Name = "Updated";
        var result = await repo.Update(item.CustomerID, item);
        Assert.Equal("Updated", result.Name);
    }

    [Fact]
    public async Task Delete_ShouldRemoveEntity()
    {
        using var context = GetContext();
        var repo = new CustomerRepository(context);
        var item = await repo.Add(new Customer { Name = "Test" });
        var deleted = await repo.Delete(item.CustomerID);
        Assert.Equal(item.CustomerID, deleted.CustomerID);
    }

    [Fact]
    public async Task GetAll_ShouldReturnPaginatedEntities()
    {
        using var context = GetContext();
        var repo = new CustomerRepository(context);
        for (int i = 0; i < 5; i++) await repo.Add(new Customer { Name = "Test" });
        var result = await repo.GetAll(1, 2);
        Assert.Equal(2, result.Count());
    }
}

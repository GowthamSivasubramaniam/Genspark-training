using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using VSM.Interfaces;
using VSM.Models;
using VSM.Services;
using Xunit;

public class CategoryServiceTests
{
    private readonly Mock<IRepository<Guid, ServiceCategory>> _categoryRepoMock;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
        _categoryRepoMock = new Mock<IRepository<Guid, ServiceCategory>>();
        _categoryService = new CategoryService(_categoryRepoMock.Object);
    }

    [Fact]
    public async Task AddCategory_Should_Return_Category_When_Successful()
    {
        var category = new ServiceCategory { Name = "Oil Change", Amount = 500 };

        _categoryRepoMock.Setup(r => r.Add(It.IsAny<ServiceCategory>()))
                         .ReturnsAsync(category);

        var result = await _categoryService.AddCategory("Oil Change", 500);

        Assert.NotNull(result);
        Assert.Equal("Oil Change", result.Name);
        Assert.Equal(500, result.Amount);
    }

    [Fact]
    public async Task AddCategory_Should_Throw_Exception_If_Add_Fails()
    {
        _categoryRepoMock.Setup(r => r.Add(It.IsAny<ServiceCategory>()))
                         .ReturnsAsync((ServiceCategory)null!);

        var ex = await Assert.ThrowsAsync<Exception>(() => _categoryService.AddCategory("Tire Rotation", 300));
        Assert.Equal("Unable to add category", ex.Message);
    }

    [Fact]
    public async Task DeleteCategory_Should_Return_True_When_Successful()
    {
        var id = Guid.NewGuid();
        var category = new ServiceCategory { CategoryID = id, Name = "Brake Check", Amount = 200 };

        _categoryRepoMock.Setup(r => r.GetAll(1, 100))
                         .ReturnsAsync(new List<ServiceCategory> { category });

        _categoryRepoMock.Setup(r => r.Update(id, It.IsAny<ServiceCategory>()))
                         .ReturnsAsync(category);

        _categoryRepoMock.Setup(r => r.Delete(id))
                         .ReturnsAsync(category);

        var result = await _categoryService.DeleteCategory(id);
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteCategory_Should_Throw_Exception_If_Category_Not_Found()
    {
        _categoryRepoMock.Setup(r => r.GetAll(1, 100))
                         .ReturnsAsync(new List<ServiceCategory>());

        var ex = await Assert.ThrowsAsync<Exception>(() => _categoryService.DeleteCategory(Guid.NewGuid()));
        Assert.Equal("Category not found", ex.Message);
    }

    [Fact]
    public async Task DeleteCategory_Should_Throw_Exception_If_Delete_Fails()
    {
        var id = Guid.NewGuid();
        var category = new ServiceCategory { CategoryID = id, Name = "AC Service", Amount = 800 };

        _categoryRepoMock.Setup(r => r.GetAll(1, 100))
                         .ReturnsAsync(new List<ServiceCategory> { category });

        _categoryRepoMock.Setup(r => r.Update(id, It.IsAny<ServiceCategory>()))
                         .ReturnsAsync(category);

        _categoryRepoMock.Setup(r => r.Delete(id))
                         .ReturnsAsync((ServiceCategory)null!);

        var ex = await Assert.ThrowsAsync<Exception>(() => _categoryService.DeleteCategory(id));
        Assert.Equal("Category cannot be deleted", ex.Message);
    }
}

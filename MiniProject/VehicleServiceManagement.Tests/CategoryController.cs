using Microsoft.AspNetCore.Mvc;
using Moq;
using VSM.Controllers;
using VSM.Interfaces;
using VSM.Misc;
using VSM.Models;
using Xunit;
using VSM.Misc;
public class CategoryControllerTests
{
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly Mock<IFileLogger> _loggerMock;
    private readonly CategoryController _controller;

    public CategoryControllerTests()
    {
        _categoryServiceMock = new Mock<ICategoryService>();
        _loggerMock = new Mock<IFileLogger>();
        _controller = new CategoryController(_categoryServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task AddCategory_ShouldReturnOk_WhenCategoryIsAdded()
    {
        // Arrange
        var category = new ServiceCategory { CategoryID = Guid.NewGuid(), Name = "Test", Amount = 100 };
        _categoryServiceMock.Setup(s => s.AddCategory("Test", 100)).ReturnsAsync(category);

        // Act
        var result = await _controller.AddCategory("Test", 100);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCategory = Assert.IsType<ServiceCategory>(okResult.Value);
        Assert.Equal("Test", returnedCategory.Name);
    }

    

   

    [Fact]
    public async Task DeleteCategory_ShouldReturnNotFound_WhenExceptionIsThrown()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _categoryServiceMock.Setup(s => s.DeleteCategory(categoryId))
                            .ThrowsAsync(new Exception("Category not found"));

        // Act
        var result = await _controller.DeleteCategory(categoryId);

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("Category not found", notFound.Value.ToString());
    }
}

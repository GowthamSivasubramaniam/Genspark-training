using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using VSM.DTO;
using VSM.Interfaces;
using VSM.Misc.Mappers;
using VSM.Models;
using VSM.Services;
using Xunit;

public class CustomerServiceTests
{
    private readonly Mock<IRepository<Guid, Customer>> _customerRepoMock;
    private readonly Mock<IRepository<string, User>> _userRepoMock;
    private readonly Mock<IEncryptionService> _encryptionServiceMock;
    private readonly Mock<IDbContextTransaction> _dbTransactionMock;
    private readonly CustomerService _customerService;

    public CustomerServiceTests()
    {
        _customerRepoMock = new Mock<IRepository<Guid, Customer>>();
        _userRepoMock = new Mock<IRepository<string, User>>();
        _encryptionServiceMock = new Mock<IEncryptionService>();
        _dbTransactionMock = new Mock<IDbContextTransaction>();

        _customerRepoMock.Setup(r => r.BeginTransaction())
                         .Returns(_dbTransactionMock.Object);

        _customerService = new CustomerService(
            _customerRepoMock.Object,
            _userRepoMock.Object,
            _encryptionServiceMock.Object);
    }

    [Fact]
    public async Task AddCustomer_Should_Add_Customer_And_User()
    {
        // Arrange
        var dto = new CustomerAddDto
        {
            Email = "test@example.com",
            Password = "password",
            Name = "Test User",
            Phone = "1234567890"
        };

        var encrypted = new EncryptModel
        {
            EncryptedData = new byte[] { 1, 2, 3 },
            HashKey = new byte[] { 4, 5, 6 }
        };

        _encryptionServiceMock.Setup(s => s.EncryptData(It.IsAny<EncryptModel>()))
                              .ReturnsAsync(encrypted);

        _userRepoMock.Setup(r => r.Get(dto.Email))
                     .ReturnsAsync((User)null);

        _userRepoMock.Setup(r => r.Add(It.IsAny<User>()))
                     .ReturnsAsync(new User { Email = dto.Email });

        _customerRepoMock.Setup(r => r.Add(It.IsAny<Customer>()))
                         .ReturnsAsync((Customer c) => c);

        _dbTransactionMock.Setup(t => t.CommitAsync(default))
                          .Returns(Task.CompletedTask);

        // Act
        var result = await _customerService.AddCustomer(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Email, result.Email);

        _userRepoMock.Verify(r => r.Get(dto.Email), Times.Once);
        _userRepoMock.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
        _customerRepoMock.Verify(r => r.Add(It.IsAny<Customer>()), Times.Once);
        _dbTransactionMock.Verify(t => t.CommitAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteCustomer_Should_Set_Status_Deleted_And_Deactivate_User()
    {
        // Arrange
        var email = "test@example.com";
        var customerId = Guid.NewGuid();

        var customerList = new List<Customer>
        {
            new Customer
            {
                CustomerID = customerId,
                Email = email,
                Status = "Active"
            }
        };

        _customerRepoMock.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>()))
                         .ReturnsAsync(customerList);

        var user = new User
        {
            Email = email,
            IsActive = true
        };

        _userRepoMock.Setup(r => r.Get(email))
                     .ReturnsAsync(user);

        _userRepoMock.Setup(r => r.Update(email, It.IsAny<User>()))
                     .ReturnsAsync(user);

        _customerRepoMock.Setup(r => r.Update(customerId, It.IsAny<Customer>()))
                         .ReturnsAsync(customerList[0]);

        // Act
        var result = await _customerService.DeleteCustomer(email);

        // Assert
        Assert.True(result);
        Assert.Equal("Deleted", customerList[0].Status);
        Assert.False(user.IsActive);

        _userRepoMock.Verify(r => r.Get(email), Times.Once);
        _userRepoMock.Verify(r => r.Update(email, It.IsAny<User>()), Times.Once);
        _customerRepoMock.Verify(r => r.Update(customerId, It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public async Task GetByEmail_Should_Return_CustomerDisplayDto()
    {
        // Arrange
        var email = "test@example.com";

        var customerList = new List<Customer>
        {
            new Customer
            {
                CustomerID = Guid.NewGuid(),
                Email = email,
                Status = "Active",
                Name = "Test User"
            }
        };

        _customerRepoMock.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>()))
                         .ReturnsAsync(customerList);

        // Act
        var result = await _customerService.GetByEmail(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
    }

    [Fact]
    public async Task GetByName_Should_Return_Customers_Matching_Name()
    {
        // Arrange
        var name = "Test";

        var customerList = new List<Customer>
        {
            new Customer
            {
                CustomerID = Guid.NewGuid(),
                Name = "Test User",
                Status = "Active"
            },
            new Customer
            {
                CustomerID = Guid.NewGuid(),
                Name = "Another User",
                Status = "Active"
            }
        };

        _customerRepoMock.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>()))
                         .ReturnsAsync(customerList);

        // Act
        var result = await _customerService.GetByName(name);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, c => c.Name.Contains(name));
    }

    [Fact]
    public async Task UpdateCustomer_Should_Update_And_Return_CustomerDisplayDto()
    {
        // Arrange
        var email = "test@example.com";
        var customerId = Guid.NewGuid();

        var customerList = new List<Customer>
        {
            new Customer
            {
                CustomerID = customerId,
                Email = email,
                Name = "Old Name",
                Status = "Active",
                Phone = "1111111111"
            }
        };

        _customerRepoMock.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>()))
                         .ReturnsAsync(customerList);

        _customerRepoMock.Setup(r => r.Update(customerId, It.IsAny<Customer>()))
                         .ReturnsAsync((Guid id, Customer c) => c);

        var updateDto = new CustomerUpdateDto
        {
            Name = "New Name",
            Phone = "2222222222"
        };

        // Act
        var result = await _customerService.UpdateCustomer(email, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updateDto.Name, result.Name);
        Assert.Equal(updateDto.Phone, result.Phone);
    }
}

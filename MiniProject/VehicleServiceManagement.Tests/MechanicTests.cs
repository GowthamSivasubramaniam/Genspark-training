using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using VSM.DTO;
using VSM.Interfaces;
using VSM.Misc.Mappers;
using VSM.Models;
using VSM.Services;
using Xunit;

public class MechanicServicesTests
{
    private readonly Mock<IRepository<Guid, Mechanic>> _mechanicRepoMock;
    private readonly Mock<IRepository<string, User>> _userRepoMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IEncryptionService> _encryptionServiceMock;
    private readonly Mock<IDbContextTransaction> _dbTransactionMock;
    private readonly Mock<ILogger<MechanicServices>> _loggerMock;
    private readonly MechanicServices _mechanicService;

    public MechanicServicesTests()
    {
        _mechanicRepoMock = new Mock<IRepository<Guid, Mechanic>>();
        _userRepoMock = new Mock<IRepository<string, User>>();
        _tokenServiceMock = new Mock<ITokenService>();
        _encryptionServiceMock = new Mock<IEncryptionService>();
        _dbTransactionMock = new Mock<IDbContextTransaction>();
        _loggerMock = new Mock<ILogger<MechanicServices>>();

        _mechanicRepoMock.Setup(r => r.BeginTransaction())
                         .Returns(_dbTransactionMock.Object);

        _mechanicService = new MechanicServices(
            _mechanicRepoMock.Object,
            _userRepoMock.Object,
            _tokenServiceMock.Object,
            _encryptionServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task AddMechanic_Should_Add_Mechanic_And_User()
    {
        // Arrange
        var dto = new MechanicAddDto
        {
            Email = "mech@example.com",
            Password = "password",
            Name = "Mechanic One",
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

        _mechanicRepoMock.Setup(r => r.Add(It.IsAny<Mechanic>()))
                         .ReturnsAsync((Mechanic m) => m);

        _dbTransactionMock.Setup(t => t.CommitAsync(default))
                          .Returns(Task.CompletedTask);

        // Act
        var result = await _mechanicService.AddMechanic(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Email, result.Email);

        _userRepoMock.Verify(r => r.Get(dto.Email), Times.Once);
        _userRepoMock.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
        _mechanicRepoMock.Verify(r => r.Add(It.IsAny<Mechanic>()), Times.Once);
        _dbTransactionMock.Verify(t => t.CommitAsync(default), Times.Once);
    }


    [Fact]
    public async Task AddMechanic_Should_Throw_Exception_If_User_Exists()
    {
        // Arrange
        var dto = new MechanicAddDto
        {
            Email = "mech@example.com",
            Password = "password",
            Name = "Mechanic One",
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
                     .ReturnsAsync(new User { Email = dto.Email });

       
        var ex = await Assert.ThrowsAsync<Exception>(() => _mechanicService.AddMechanic(dto));
        Assert.Equal("User already exists", ex.Message);
    }


    [Fact]
    public async Task DeleteMechanic_Should_Set_Status_Deleted_And_Deactivate_User()
    {
        // Arrange
        var email = "mech@example.com";
        var mechanicId = Guid.NewGuid();

        var mechanicList = new List<Mechanic>
        {
            new Mechanic
            {
                MechanicId = mechanicId,
                Email = email,
                Status = "Active"
            }
        };

        _mechanicRepoMock.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>()))
                         .ReturnsAsync(mechanicList);

        var user = new User { Email = email, IsActive = true };

        _userRepoMock.Setup(r => r.Get(email)).ReturnsAsync(user);

        _userRepoMock.Setup(r => r.Update(email, It.IsAny<User>())).ReturnsAsync(user);

        _mechanicRepoMock.Setup(r => r.Update(mechanicId, It.IsAny<Mechanic>()))
                         .ReturnsAsync(mechanicList[0]);

        // Act
        var result = await _mechanicService.DeleteMechanic(email);

        // Assert
        Assert.True(result);
        Assert.Equal("Deleted", mechanicList[0].Status);
        Assert.False(user.IsActive);

        _userRepoMock.Verify(r => r.Get(email), Times.Once);
        _userRepoMock.Verify(r => r.Update(email, It.IsAny<User>()), Times.Once);
        _mechanicRepoMock.Verify(r => r.Update(mechanicId, It.IsAny<Mechanic>()), Times.Once);
    }

    [Fact]
    public async Task DeleteMechanic_Should_Throw_If_Mechanic_Not_Found()
    {
        // Arrange
        _mechanicRepoMock.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>()))
                         .ReturnsAsync(new List<Mechanic>());

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _mechanicService.DeleteMechanic("unknown@example.com"));
        Assert.Contains("not found", ex.Message);
    }

    [Fact]
    public async Task GetByEmail_Should_Return_MechanicDisplayDto()
    {
        // Arrange
        var email = "mech@example.com";
        var mechanicList = new List<Mechanic>
        {
            new Mechanic
            {
                MechanicId = Guid.NewGuid(),
                Email = email,
                Status = "Active",
                Name = "Mech One"
            }
        };

        _mechanicRepoMock.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>()))
                         .ReturnsAsync(mechanicList);

        // Act
        var result = await _mechanicService.GetByEmail(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
    }

    [Fact]
    public async Task GetByName_Should_Return_Filtered_Mechanics()
    {
        // Arrange
        var name = "Mech";

        var mechanicList = new List<Mechanic>
        {
            new Mechanic { MechanicId = Guid.NewGuid(), Name = "Mech One", Status = "Active" },
            new Mechanic { MechanicId = Guid.NewGuid(), Name = "Other", Status = "Active" }
        };

        _mechanicRepoMock.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>()))
                         .ReturnsAsync(mechanicList);

        // Act
        var result = await _mechanicService.GetByName(name);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, m => m.Name.Contains(name));
    }

    [Fact]
    public async Task GetAll_Should_Return_All_Active_Mechanics()
    {
        // Arrange
        var mechanicList = new List<Mechanic>
        {
            new Mechanic { MechanicId = Guid.NewGuid(), Name = "Mech One", Status = "Active" },
            new Mechanic { MechanicId = Guid.NewGuid(), Name = "Mech Two", Status = "Active" }
        };

        _mechanicRepoMock.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>()))
                         .ReturnsAsync(mechanicList);

        // Act
        var result = await _mechanicService.GetAll();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateMechanic_Should_Update_And_Return_MechanicDisplayDto()
    {
        // Arrange
        var email = "mech@example.com";
        var mechanicId = Guid.NewGuid();

        var mechanicList = new List<Mechanic>
        {
            new Mechanic
            {
                MechanicId = mechanicId,
                Email = email,
                Name = "Old Name",
                Status = "Active",
                Phone = "1111111111"
            }
        };

        _mechanicRepoMock.Setup(r => r.GetAll(It.IsAny<int>(), It.IsAny<int>()))
                         .ReturnsAsync(mechanicList);

        _mechanicRepoMock.Setup(r => r.Update(mechanicId, It.IsAny<Mechanic>()))
                         .ReturnsAsync((Guid id, Mechanic m) => m);

        var updateDto = new MechanicUpdateDto
        {
            Name = "New Name",
            Phone = "2222222222"
        };

        // Act
        var result = await _mechanicService.UpdateMechanic(email, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updateDto.Name, result.Name);
        Assert.Equal(updateDto.Phone, result.Phone);
    }
}

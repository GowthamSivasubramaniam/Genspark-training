using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSM.Contexts;
using VSM.Models;
using VSM.Repositories;
using Xunit;

namespace VSM.Tests.Repositories
{
    public class VehicleRepositoryTests
    {
        private readonly VSMContext _context;
        private readonly VehicleRepository _repository;

        public VehicleRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<VSMContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new VSMContext(options);
            _context.Database.EnsureCreated();
            _repository = new VehicleRepository(_context);
        }

        [Fact]
        public async Task Add_ShouldAddVehicle()
        {
            var vehicle = new Vehicle { VehicleNo = "TN01AB1234", VehicleType = "Car", VechicleManufacturer = "Toyota", VehicleModel = "Etios" };
            var result = await _repository.Add(vehicle);

            Assert.NotNull(result);
            Assert.Equal("TN01AB1234", result.VehicleNo);
        }

        [Fact]
        public async Task Get_ShouldReturnExistingVehicle()
        {
            var vehicle = new Vehicle { VehicleNo = "TN01AB5678", VehicleType = "Bike", VechicleManufacturer = "Honda", VehicleModel = "Unicorn" };
            await _repository.Add(vehicle);

            var result = await _repository.Get(vehicle.VehicleID);
            Assert.NotNull(result);
            Assert.Equal("TN01AB5678", result.VehicleNo);
        }

        [Fact]
        public async Task Get_ShouldNotReturnNonExistingVehicle()
        {
            var result = await _repository.Get(Guid.NewGuid());
            Assert.Null(result);
        }

        [Fact]
        public async Task Delete_ShouldRemoveVehicle()
        {
            var vehicle = new Vehicle { VehicleNo = "TN01AB9999", VehicleType = "SUV", VechicleManufacturer = "Mahindra", VehicleModel = "Scorpio" };
            await _repository.Add(vehicle);

            var result = await _repository.Delete(vehicle.VehicleID);
            Assert.NotNull(result);

            var deleted = await _repository.Get(vehicle.VehicleID);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task Delete_ShouldNotThrowForNonExistingVehicle()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await _repository.Delete(Guid.NewGuid());
            });
        }

        [Fact]
        public async Task Update_ShouldModifyVehicleDetails()
        {
            var vehicle = new Vehicle { VehicleNo = "TN02AA1111", VehicleType = "Truck", VechicleManufacturer = "Tata", VehicleModel = "Ace" };
            await _repository.Add(vehicle);

            vehicle.VehicleModel = "Super Ace";
            var updated = await _repository.Update(vehicle.VehicleID, vehicle);

            Assert.NotNull(updated);
            Assert.Equal("Super Ace", updated.VehicleModel);
        }

        [Fact]
        public async Task Update_ShouldNotUpdateNonExistingVehicle()
        {
            var vehicle = new Vehicle { VehicleID = Guid.NewGuid(), VehicleNo = "TN03BB0001", VehicleType = "Bus", VechicleManufacturer = "Ashok Leyland", VehicleModel = "Viking" };
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await _repository.Update(vehicle.VehicleID, vehicle);
            });
        }

        [Fact]
        public async Task GetAll_ShouldReturnPaginatedVehicles()
        {
            for (int i = 0; i < 15; i++)
            {
                await _repository.Add(new Vehicle { VehicleNo = $"TN99Z{i:D4}", VehicleType = "Auto", VechicleManufacturer = "Bajaj", VehicleModel = "RE" });
            }

            var page1 = await _repository.GetAll(1, 10);
            var page2 = await _repository.GetAll(2, 10);

            Assert.Equal(10, page1.Count());
            Assert.Equal(5, page2.Count());
        }
    }

    // Repository implementation (same as yours for context)
    public class VehicleRepository : Repository<Guid, Vehicle>
    {
        public VehicleRepository(VSMContext context) : base(context) { }

        public override async Task<Vehicle> Get(Guid key)
        {
            return await _context.Set<Vehicle>().FindAsync(key);
        }

        public override async Task<IEnumerable<Vehicle>> GetAll(int pageNumber, int pageSize)
        {
            return await _context.Set<Vehicle>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}

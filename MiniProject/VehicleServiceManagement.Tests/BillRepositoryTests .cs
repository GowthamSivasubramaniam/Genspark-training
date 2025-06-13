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
    public class RepositoryTestsFixture : IDisposable
    {
        public readonly VSMContext Context;

        public RepositoryTestsFixture()
        {
            var options = new DbContextOptionsBuilder<VSMContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            Context = new VSMContext(options);
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }

    public class BillRepositoryTests : IClassFixture<RepositoryTestsFixture>
    {
        private readonly BillRepository _repo;
        private readonly VSMContext _context;

        public BillRepositoryTests(RepositoryTestsFixture fixture)
        {
            _context = fixture.Context;
            _repo = new BillRepository(_context);
        }

        [Fact]
        public async Task Should_Add_Bill()
        {
            var bill = new Bill();
            var result = await _repo.Add(bill);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldNot_Get_Invalid_Bill()
        {
            await Assert.ThrowsAsync<Exception>(async () => await _repo.Get(Guid.NewGuid()));
        }
    }

    public class CustomerRepositoryTests : IClassFixture<RepositoryTestsFixture>
    {
        private readonly CustomerRepository _repo;
        private readonly VSMContext _context;

        public CustomerRepositoryTests(RepositoryTestsFixture fixture)
        {
            _context = fixture.Context;
            _repo = new CustomerRepository(_context);
        }

        [Fact]
        public async Task Should_Add_Customer()
        {
            var customer = new Customer { Name = "John Doe" };
            var result = await _repo.Add(customer);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldNot_Get_Invalid_Customer()
        {
            await Assert.ThrowsAsync<Exception>(async () => await _repo.Get(Guid.NewGuid()));
        }
    }

    public class BookingRepositoryTests : IClassFixture<RepositoryTestsFixture>
    {
        private readonly BookingRepository _repo;
        private readonly VSMContext _context;

        public BookingRepositoryTests(RepositoryTestsFixture fixture)
        {
            _context = fixture.Context;
            _repo = new BookingRepository(_context);
        }

        [Fact]
        public async Task Should_Add_Booking()
        {
            var booking = new Booking { Slot = DateTime.UtcNow };
            var result = await _repo.Add(booking);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldNot_Get_Invalid_Booking()
        {
            await Assert.ThrowsAsync<Exception>(async () => await _repo.Get(Guid.NewGuid()));
        }
    }

    public class MechanicRepositoryTests : IClassFixture<RepositoryTestsFixture>
    {
        private readonly MechanicRepository _repo;
        private readonly VSMContext _context;

        public MechanicRepositoryTests(RepositoryTestsFixture fixture)
        {
            _context = fixture.Context;
            _repo = new MechanicRepository(_context);
        }

        [Fact]
        public async Task Should_Add_Mechanic()
        {
            var mechanic = new Mechanic { Name = "Mike" };
            var result = await _repo.Add(mechanic);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldNot_Get_Invalid_Mechanic()
        {
            await Assert.ThrowsAsync<Exception>(async () => await _repo.Get(Guid.NewGuid()));
        }
    }

    public class ServiceCategoryRepositoryTests : IClassFixture<RepositoryTestsFixture>
    {
        private readonly ServiceCategoriesRepository _repo;
        private readonly VSMContext _context;

        public ServiceCategoryRepositoryTests(RepositoryTestsFixture fixture)
        {
            _context = fixture.Context;
            _repo = new ServiceCategoriesRepository(_context);
        }

        [Fact]
        public async Task Should_Add_Category()
        {
            var category = new ServiceCategory { Name = "Oil Change" };
            var result = await _repo.Add(category);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldNot_Get_Invalid_Category()
        {
            await Assert.ThrowsAsync<Exception>(async () => await _repo.Get(Guid.NewGuid()));
        }
    }
}

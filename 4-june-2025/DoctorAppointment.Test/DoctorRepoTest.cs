using System.Threading.Tasks;
using DoctorAppointment.Contexts;
using DoctorAppointment.Interfaces;
using DoctorAppointment.Models;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointment.Test;

public class Tests
{

    private ClinicContext _context;
    private ClinicContext _context_null;
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicContext>()
       .UseInMemoryDatabase ("TestDB").Options;
        var options1 = new DbContextOptionsBuilder<ClinicContext>()
       .UseInMemoryDatabase ("TestDB1").Options;

        _context = new ClinicContext(options);
        _context_null = new ClinicContext(options1);
       
    }

    [Test]
    public void UserRepoTest()
    {
        //Arrange

        var user = new User
        {
            Username = "Abcde@gmail.com",
            Password = System.Text.Encoding.UTF8.GetBytes("12345"),
            HashKey = Guid.NewGuid().ToByteArray(),
            Role = "Doctor"

        };

        //Action
        var doc = _context.Add(user);
        _context.SaveChanges();
        //Assert
        Assert.That(doc, Is.Not.Null, "Doctor cannot be added");


    }
    [Test]
    public void DoctorRepoTest()
    {   IRepository<int, Doctor> repo = new DoctorRepo(_context);
        var doc = new Doctor
        {
            Name = "abs",
            YearsOfExperience = 10,
            Email = "Abcde@gmail.com",
            Status = "Active"
        };
        var doct = repo.Add(doc);
         _context.SaveChanges();
        //Assert
        Assert.That(doct, Is.Not.Null, $"{doct}");
        // Assert.That(doct, Is.Null, $"{doct}");
    }
    [TestCase(109090)]
    public async Task GetDoctorbyIdTest_Not_Found(int id)
    {
        IRepository<int, Doctor> repo = new DoctorRepo(_context);
        // var doc = await repo.Get(id);
        Assert.ThrowsAsync<Exception>(async () =>
        {
            await repo.Get(id);
        });
         
    }
    [TestCase(1)]
    public async Task GetDoctorbyIdTest_Found(int id)
    {
        IRepository<int, Doctor> repo = new DoctorRepo(_context);
        var doc = await repo.Get(id);
        Assert.That(doc, Is.Not.Null);
         
    }

    [Test]
    public async Task GetAllDoctors_NotFound()
    {
        IRepository<int, Doctor> repo = new DoctorRepo(_context_null);
        // var doc = await repo.Get(id);
        Assert.ThrowsAsync<Exception>(async () =>
        {
            await repo.GetAll();
        });

    }
    [Test]
    public async Task GetAllDoctors_Found()
    {
        IRepository<int, Doctor> repo = new DoctorRepo(_context);
        var doc = await repo.GetAll();

        Assert.That(doc.Count(), Is.EqualTo(1));

    }
    [TestCase(1)]
    public async Task UpdateDoctor_Success(int id)
    {
        IRepository<int, Doctor> repo = new DoctorRepo(_context);
         var doc = new Doctor
        {
            Name = "abs",
            YearsOfExperience = 10,
            Email = "Abcde@gmail.com",
            Status = "Active"
        };

         var doc1 = await repo.Update(id,doc);

         Assert.That(doc1, Is.Not.Null);
         
    }
    [Test]
    public async Task DeleteDoctor_Success()
    {
        
         IRepository<int, Doctor> repo = new DoctorRepo(_context_null);
          var doc = new Doctor
        {
            Name = "abs",
            YearsOfExperience = 10,
            Email = "Abcde@gmail.com",
            Status = "Active"
        };
          await repo.Add(doc);
         _context_null.SaveChanges();
         var doc1 = await repo.Delete(1);
         Assert.That(doc1, Is.Not.Null);

    }
   
    [TearDown]
    public void TearDown()
    {
        
        _context.Dispose();
        _context_null.Dispose();
    }
}

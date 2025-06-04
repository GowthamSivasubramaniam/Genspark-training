using System.Threading.Tasks;
using DoctorAppointment.Contexts;
using DoctorAppointment.Interfaces;
using DoctorAppointment.Models;
using DoctorAppointment.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DoctorAppointment.Test;

public class PatientTests
{
    private ClinicContext _context;
    private ClinicContext _context_null;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicContext>()
            .UseInMemoryDatabase("TestDB2").Options;
        var options1 = new DbContextOptionsBuilder<ClinicContext>()
            .UseInMemoryDatabase("TestDB3").Options;

        _context = new ClinicContext(options);
        _context_null = new ClinicContext(options1);
    }

    [Test]
    public async Task UserRepoTest()
    {
        var user = new User
        {
            Username = "Patient@gmail.com",
            Password = System.Text.Encoding.UTF8.GetBytes("12345"),
            HashKey = Guid.NewGuid().ToByteArray(),
            Role = "Patient"
        };

        var pat =  _context.Add(user);
         _context.SaveChanges();

        Assert.That(pat, Is.Not.Null, "Patient cannot be added");
    }

    [Test]
    public async Task PatientRepoTest()
    {
        IRepository<int, Patient> repo = new PatientRepo(_context);
        var pat = new Patient
        {
            Name = "patient1",
            Age = 30,
            Email = "Patient@gmail.com",
            
        };
        var saved = await repo.Add(pat);
      
        Assert.That(saved, Is.Not.Null, $"{saved}");
    }

    [TestCase(109090)]
    public async Task GetPatientbyIdTest_Not_Found(int id)
    {
        IRepository<int, Patient> repo = new PatientRepo(_context);
        Assert.ThrowsAsync<Exception>(async () =>
        {
            await repo.Get(id);
        });
    }

    [TestCase(1)]
    public async Task GetPatientbyIdTest_Found(int id)
    {
        IRepository<int, Patient> repo = new PatientRepo(_context);
         var pat = new Patient
        {
            Name = "patient1",
            Age = 30,
            Email = "Patient@gmail.com",
            
        };
        var saved = await repo.Add(pat);
        var pat1 = await repo.Get(id);
        Assert.That(pat1, Is.Not.Null);
    }

    [Test]
    public async Task GetAllPatients_NotFound()
    {
        IRepository<int, Patient> repo = new PatientRepo(_context_null);

        Assert.ThrowsAsync<Exception>(async () =>
        {
            await repo.GetAll();
        });
    }

    [Test]
    public async Task GetAllPatients_Found()
    {
        IRepository<int, Patient> repo = new PatientRepo(_context);
        // IRepository<int, Patient> repo = new PatientRepo(_context);
         var pat = new Patient
        {
            Name = "patient1",
            Age = 30,
            Email = "Patient@gmail.com",
            
        };
        var saved = await repo.Add(pat);
       
        var pats = await repo.GetAll();
        Assert.That(pats.Count(), Is.EqualTo(1));
    }

    [TestCase(1)]
    public async Task UpdatePatient_Success(int id)
    {
        IRepository<int, Patient> repo = new PatientRepo(_context);
        var pat = new Patient
        {
            Name = "patient1",
            Age = 30,
            Email = "Patient@gmail.com",
            
        };

        var updated = await repo.Update(id, pat);
        Assert.That(updated, Is.Not.Null);
    }

    [Test]
    public async Task DeletePatient_Success()
    {
        IRepository<int, Patient> repo = new PatientRepo(_context_null);
        var pat = new Patient
        {
            Name = "patient1",
            Age = 30,
            Email = "Patient@gmail.com",
           
        };
        await repo.Add(pat);
        _context_null.SaveChanges();
        var deleted = await repo.Delete(1);
        Assert.That(deleted, Is.Not.Null);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        _context_null.Dispose();
    }
}

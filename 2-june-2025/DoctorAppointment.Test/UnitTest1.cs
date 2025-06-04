// using System.Threading.Tasks;
// using DoctorAppointment.Contexts;
// using DoctorAppointment.Interfaces;
// using DoctorAppointment.Models;
// using Microsoft.AspNetCore.Mvc.DataAnnotations;
// using Microsoft.EntityFrameworkCore;

// namespace DoctorAppointment.Test;

// public class Tests
// {

//     private ClinicContext _context;
//     [SetUp]
//     public void Setup()
//     {
//         var options = new DbContextOptionsBuilder<ClinicContext>()
//        .UseNpgsql("User ID=postgres;Password=gowthamm1@;Host=localhost;Port=5432;Database=ClinicApp1;").Options;

//         _context = new ClinicContext(options);
//           _context.Database.OpenConnection();   // <-- Add this
//     _context.Database.EnsureCreated();    //
//     }

//     // [Test]
//     // public void UserRepoTest()
//     // {
//     //     //Arrange

//     //     var user = new User
//     //     {
//     //         Username = "Abcde@gmail.com",
//     //         Password = System.Text.Encoding.UTF8.GetBytes("12345"),
//     //         HashKey = Guid.NewGuid().ToByteArray(),
//     //         Role = "Doctor"

//     //     };

//     //     //Action
//     //     var doc = _context.Add(user);
//     //     _context.SaveChanges();
//     //     //Assert
//     //     Assert.That(doc, Is.Not.Null, "Doctor cannot be added");


//     // }
//     // [Test]
//     // public void DoctorRepoTest()
//     // {
//     //     var doc = new Doctor
//     //     {
//     //         Name = "abs",
//     //         YearsOfExperience = 10,
//     //         Email = "Abcde@gmail.com",
//     //         Status = "Active"
//     //     };
//     //     var doct = _context.Add(doc);
//     //      _context.SaveChanges();
//     //     //Assert
//     //     Assert.That(doct, Is.Not.Null, $"{doct}");
//     //     // Assert.That(doct, Is.Null, $"{doct}");
//     // }
//     [TestCase(580980980)]
//     public async Task GetDoctorbyIdTest(int id)
//     {
//         IRepository<int, Doctor> repo = new DoctorRepo(_context);
//         // var doc = await repo.Get(id);
//         Assert.ThrowsAsync<Exception>(async () =>
//         {
//             await repo.Get(id);
//         });
         
//     }
   
//     [TearDown]
//     public void TearDown()
//     {
//         _context.Database.CloseConnection();
//         _context.Dispose();
//     }
// }

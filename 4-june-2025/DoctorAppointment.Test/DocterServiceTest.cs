using System.Threading.Tasks;
using DoctorAppointment.Contexts;
using DoctorAppointment.Interfaces;
using DoctorAppointment.Models;
using DoctorAppointment.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Moq;
using DoctorAppointment.Services;
using FirstAPI.Repositories;
using DoctorAppointment.Service;
namespace DoctorAppointment.Test;

public class ServiceTest
{

    private ClinicContext _context;
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicContext>()
       .UseInMemoryDatabase("TestDB").Options;

        _context = new ClinicContext(options);
       
    }

    [TestCase("abs")]
    public async Task GetDoctorbyname_Found(string s)
    {
        Mock<DoctorRepo> doctorRepositoryMock = new Mock<DoctorRepo>(_context);
        Mock<SpecialityRepo> specialityRepositoryMock = new(_context);
        Mock<DoctorSpecialityRepo> doctorSpecialityRepositoryMock = new(_context);
        Mock<UserRepository> userRepositoryMock = new(_context);
        Mock<EncryptionService> encryptionServiceMock = new();
        Mock<IMapper> mapperMock = new();
        
  
     doctorRepositoryMock
    .Setup(repo => repo.GetAll())
    .ReturnsAsync(new List<Doctor>
    {
        new Doctor { Name = "abs", YearsOfExperience = 10, Email = "Abcde@gmail.com", Status = "Active" }
    });

       
        IDoctorService doctorService = new DoctorService(doctorRepositoryMock.Object,
                                                        specialityRepositoryMock.Object,
                                                        doctorSpecialityRepositoryMock.Object,
                                                        userRepositoryMock.Object,
                                                        encryptionServiceMock.Object,
                                                        mapperMock.Object);

       
        var result = await doctorService.GetDoctByName(s);
        //Assert
        Assert.That(result, Is.Not.Null , "No Doctor found");
   }
    [TearDown]
    public void TearDown()
    {
        
        _context.Dispose();
    }
}

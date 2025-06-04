using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoctorAppointment.Interfaces;
using DoctorAppointment.Models;
using DoctorAppointment.Models.DTO;
using DoctorAppointment.Repositories;
using DoctorAppointment.Service;
using Moq;
using NUnit.Framework;

namespace DoctorAppointment.Test
{
    public class PatientServicesTest
    {
        private Mock<PatientRepo> _patientRepoMock;
        private Mock<IRepository<string, User>> _userRepoMock;
        private Mock<IEncryptionService> _encryptionServiceMock;
        private Mock<AutoMapper.IMapper> _mapperMock;
        private PatientServices _patientService;

        [SetUp]
        public void Setup()
        {
            _patientRepoMock = new Mock<PatientRepo>(null); // Pass null or your context if needed
            _userRepoMock = new Mock<IRepository<string, User>>();
            _encryptionServiceMock = new Mock<IEncryptionService>();
            _mapperMock = new Mock<AutoMapper.IMapper>();

            _patientService = new PatientServices(
                _patientRepoMock.Object,
                _userRepoMock.Object,
                _encryptionServiceMock.Object,
                _mapperMock.Object
            );
        }

        [Test]
        public async Task Add_Should_Add_Patient_Successfully()
        {
            // Arrange
            var patientDto = new PatientAddDto
            {
                Name = "John Doe",
                Age = 30,
                Email = "john@example.com",
                Phone = "1234567890",
                Password = "password123"
            };

            var user = new User
            {
                Username = patientDto.Email,
                Role = "Patient"
            };

            var encryptedModel = new EncryptModel
            {
                EncryptedData = new byte[] { 1, 2, 3 },
                HashKey = new byte[] { 4, 5, 6 }
            };

            var patient = new Patient
            {
                Name = patientDto.Name,
                Age = patientDto.Age,
                Email = patientDto.Email,
                Phone = patientDto.Phone,
                User = user
            };

            _mapperMock
                .Setup(m => m.Map<PatientAddDto, User>(patientDto))
                .Returns(user);

            _encryptionServiceMock
                .Setup(e => e.EncryptData(It.IsAny<EncryptModel>()))
                .ReturnsAsync(encryptedModel);

            _userRepoMock
                .Setup(r => r.Add(It.IsAny<User>()))
                .ReturnsAsync(user);

            _patientRepoMock
                .Setup(r => r.Add(It.IsAny<Patient>()))
                .ReturnsAsync(patient);

            // Act
            var result = await _patientService.Add(patientDto);

            // Assert
         Assert.That(result,Is.Not.Null," ");
            // Assert.AreEqual(patientDto.Name, result.Name);
            // Assert.AreEqual(patientDto.Email, result.Email);
            // Assert.AreEqual(user, result.User);
        }

        [Test]
        public async Task Get_Should_Return_Patient()
        {
            // Arrange
            var patientId = 1;
            var patient = new Patient { Name = "John Doe", Email = "john@example.com" };

            _patientRepoMock
                .Setup(r => r.Get(patientId))
                .ReturnsAsync(patient);

            // Act
            var result = await _patientService.Get(patientId);

            // Assert
            Assert.That(result,Is.Not.Null," ");
            
        }

        [Test]
        public async Task GetAll_Should_Return_All_Patients()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient { Name = "John Doe" },
                new Patient { Name = "Jane Smith" }
            };

            _patientRepoMock
                .Setup(r => r.GetAll())
                .ReturnsAsync(patients);

            // Act
            var result = await _patientService.GetAll();

            // Assert

            // Assert.AreEqual(2, ((List<Patient>)result).Count);
            Assert.That(((List<Patient>)result).Count(),Is.EqualTo(2));
            
        }

        [Test]
        public async Task Update_Should_Return_Updated_Patient()
        {
            // Arrange
            var patientId = 1;
            var patient = new Patient { Name = "Updated Name" };

            _patientRepoMock
                .Setup(r => r.Update(patientId, patient))
                .ReturnsAsync(patient);

            // Act
            var result = await _patientService.Update(patientId, patient);

            // Assert
            Assert.That(result,Is.Not.Null," ");
            // Assert.AreEqual("Updated Name", result.Name);
        }

        [Test]
        public async Task Delete_Should_Return_Deleted_Patient()
        {
            // Arrange
            var patientId = 1;
            var patient = new Patient { Name = "John Doe" };

            _patientRepoMock
                .Setup(r => r.Delete(patientId))
                .ReturnsAsync(patient);

            // Act
            var result = await _patientService.Delete(patientId);

            // Assert
           Assert.That(result,Is.Not.Null," ");
            // Assert.AreEqual(patient.Name, result.Name);
        }
    }
}

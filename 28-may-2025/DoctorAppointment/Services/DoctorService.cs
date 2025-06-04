using System.Net.Http.Headers;
using DoctorAppointment.Contexts;
using DoctorAppointment.Interfaces;
using DoctorAppointment.Models;
using DoctorAppointment.Models.DTO;
using DoctorAppointment.Repositories;
using Microsoft.OpenApi.Validations;

namespace DoctorAppointment.Service
{

    public class DoctorService : IDoctorService
    {
        private readonly IRepository<int, Doctor> _drepo;
        private readonly IRepository<int, Speciality> _srepo;
        private readonly IRepository<int, DoctorSpeciality> _dsrepo;

        public DoctorService(
            IRepository<int, Doctor> doctorRepository,
            IRepository<int, Speciality> specialityRepository,
            IRepository<int, DoctorSpeciality> doctorSpecialityRepository)
        {
            _drepo = doctorRepository;
            _srepo = specialityRepository;
            _dsrepo = doctorSpecialityRepository;
        }


        public async Task<Doctor> AddDoctor(DoctorAddDto doctor)
        {
            var doc = new Doctor
            {
                Name = doctor.Name,
                Status = doctor.Status,
                YearsOfExperience = doctor.YearsOfExperience
            };

            var existingSpecialities = await _srepo.GetAll();


            var specialityDict = existingSpecialities.ToDictionary(s => s.Name.ToLower(), s => s.Id);


            var validSpecialityIds = new List<int>();


            foreach (var specName in doctor.Specialities)
            {
                if (specialityDict.TryGetValue(specName.Name.ToLower(), out var specId))
                {
                    validSpecialityIds.Add(specId);
                }
                else
                {
                    throw new Exception($"Speciality '{specName.Name}' not found.");
                }
            }


            var savedDoctor = await _drepo.Add(doc);


            foreach (var specId in validSpecialityIds)
            {
                await _dsrepo.Add(new DoctorSpeciality
                {
                    DoctorId = savedDoctor.Id,
                    SpecialityId = specId
                });
            }

            return savedDoctor;
        }

        public async Task<Doctor> GetDoctByName(string name)
        {


            var doctors = await _drepo.GetAll();
            var doctorsWithGivenName = doctors.FirstOrDefault(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (doctorsWithGivenName == null)
                throw new Exception($"Doctors with Name'{name}' not found.");
            return doctorsWithGivenName;

        }

        public async Task<ICollection<DisplayDoctorDto>> GetDoctorsBySpeciality(string speciality)
{
    var doctors = await _drepo.GetAll();

    var result = doctors
        .Where(d => d.DoctorSpecialities != null &&
                    d.DoctorSpecialities.Any(ds => 
                        ds.Speciality != null &&
                        ds.Speciality.Name.Equals(speciality, StringComparison.OrdinalIgnoreCase)))
        .Select(d => new DisplayDoctorDto
        {
            Name = d.Name,
            YearsOfExperience = d.YearsOfExperience,
            Specialities = d.DoctorSpecialities?
                .Select(ds => ds.Speciality.Name)
                .ToList() ?? new List<string>()
        })
        .ToList();

    return result;
}

        public async Task<IEnumerable<DisplayDoctorDto>> GetDoctors()
        {
            var doctors = await _drepo.GetAll();
            var result = doctors.Select(d => new DisplayDoctorDto
            {
                Name = d.Name,
                YearsOfExperience = d.YearsOfExperience,
                Specialities = d.DoctorSpecialities?
                            .Select(ds => ds.Speciality.Name)
                            .ToList() ?? new List<string>()
            });
         
            return result;

        }
    }
}
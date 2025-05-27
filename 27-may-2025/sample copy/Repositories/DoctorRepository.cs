
using DocApp.Models;
using DocApp.Interfaces;
namespace DocApp.Repositories
{
    public class DoctorRepository : IRepository<Doctor>
    {
         private static readonly List<Doctor> _Doctors = new()
        {
            new Doctor { Id = 1, Name = "Gowtham",  Specialization = "Cardiologist" },
            new Doctor { Id = 2, Name = "Gokul", Specialization = "ENT" }
        };

        public IEnumerable<Doctor> GetAll() => _Doctors;

        public Doctor GetById(int id) => _Doctors.FirstOrDefault(a => a.Id == id);

        public void Add(Doctor doctor) => _Doctors.Add(doctor);

        public void Update(int id, Doctor doctor)
        {
            var existing = GetById(id);
            if (existing != null)
            {
               
                existing.Name = doctor.Name;
                existing.Specialization = doctor.Specialization;
              
            }
        }

        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing != null)
            {
                _Doctors.Remove(existing);
            }
        }
    }
}

using DocApp.Models;
using DocApp.Interfaces;
namespace DocApp.Repositories
{
    public class PatientRepository : IRepository<Patient>
    {
         private static readonly List<Patient> _patients = new()
        {
            new Patient { Id = 1, Name = "Karthick", Phone_no ="1234567890",  Diagnosis = "Fever" },
            new Patient { Id = 2, Name = "Shiva", Phone_no ="1234567980", Diagnosis = "Cold" }
        };

        public IEnumerable<Patient> GetAll() => _patients;

        public Patient GetById(int id) => _patients.FirstOrDefault(a => a.Id == id);

        public void Add(Patient patient)
        {
            
            _patients.Add(patient);
        }

        public void Update(int id, Patient patient)
        {
            var existing = GetById(id);
            if (existing != null)
            {

                existing.Name = patient.Name;
                existing.Phone_no = patient.Phone_no;
                existing.Diagnosis = patient.Diagnosis;

            }
        }

        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing != null)
            {
                _patients.Remove(existing);
            }
        }
    }
}
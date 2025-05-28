using System.Net.Http.Headers;
using DoctorAppointment.Contexts;
using DoctorAppointment.Interfaces;
using DoctorAppointment.Models;
using DoctorAppointment.Repositories;

namespace DoctorAppointment.Service
{
    public class PatientServices : IPatientService
    {
        private PatientRepo _repo;
        public PatientServices(PatientRepo repo)
        {
            _repo = repo;
        }
        public async Task<Patient> Add(Patient patient)
        {
            await _repo.Add(patient);
            return patient;

        }

        public async Task<Patient> Delete(int key)
        {
            var patient =  await _repo.Delete(key);
            return patient;
        }

        public async Task<Patient> Get(int key)
        {
            var patient =  await _repo.Get(key);
            return patient;
        }

        public async Task<IEnumerable<Patient>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<Patient> Update(int key, Patient item)
        {
            return await _repo.Update(key, item);
        }
    }
}
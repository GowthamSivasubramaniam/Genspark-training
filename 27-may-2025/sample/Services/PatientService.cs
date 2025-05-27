using DocApp.Repositories;
using DocApp.Models;
using DocApp.Interfaces;
namespace DocApp.Services
{
public class PatientService
{
    private readonly IRepository<Patient> _repo;

    public PatientService(IRepository<Patient> repo)
    {
        _repo = repo;
    }

    public IEnumerable<Patient> GetAll()
    {
        return _repo.GetAll();
    }

    public Patient Get(int id) => _repo.GetById(id);
    public void Add(Patient patient) => _repo.Add(patient);
    public void Update(int id, Patient patient) => _repo.Update(id, patient);
    public void Delete(int id) => _repo.Delete(id);

}
}
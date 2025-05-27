using DocApp.Repositories;
using DocApp.Models;
using DocApp.Interfaces;
namespace DocApp.Services
{
public class DoctorService
{
    private readonly IRepository<Doctor> _repo;

    public DoctorService(IRepository<Doctor> repo)
    {
        _repo = repo;
    }

    public IEnumerable<Doctor> GetAll()
    {
        return _repo.GetAll();
    }

    public Doctor Get(int id) => _repo.GetById(id);
    public void Add(Doctor doctor) => _repo.Add(doctor);
    public void Update(int id, Doctor doctor) => _repo.Update(id, doctor);
    public void Delete(int id) => _repo.Delete(id);

}
}
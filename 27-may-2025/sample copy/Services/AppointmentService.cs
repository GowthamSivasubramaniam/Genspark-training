using DocApp.Repositories;
using DocApp.Models;
using DocApp.Interfaces;
namespace DocApp.Services
{
public class AppointmentService
{
    private readonly IRepository<Appointment> _repo;

    public AppointmentService(IRepository<Appointment> repo)
    {
        _repo = repo;
    }

    public IEnumerable<Appointment> GetAll()
    {
        return _repo.GetAll();
    }

    public Appointment Get(int id) => _repo.GetById(id);
    public void Add(Appointment appointment) => _repo.Add(appointment);
    public void Update(int id, Appointment appointment) => _repo.Update(id, appointment);
    public void Delete(int id) => _repo.Delete(id);

}
}
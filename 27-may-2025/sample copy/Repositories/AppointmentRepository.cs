
using DocApp.Models;
using DocApp.Interfaces;
namespace DocApp.Repositories
{
    public class AppointmentRepository : IRepository<Appointment>
    {
         private static readonly List<Appointment> _appointments = new()
        {
            new Appointment { Id = 1, PatientName = "karthick", DoctorName = "Gowtham", AppointmentDate = DateTime.Now.AddDays(1), Notes = "Regular check-up" },
            new Appointment { Id = 2, PatientName = "shiva", DoctorName = "Gokul", AppointmentDate = DateTime.Now.AddDays(2), Notes = "Follow-up" }
        };

        public IEnumerable<Appointment> GetAll() => _appointments;

        public Appointment GetById(int id) => _appointments.FirstOrDefault(a => a.Id == id);

        public void Add(Appointment appointment) => _appointments.Add(appointment);

        public void Update(int id, Appointment appointment)
        {
            var existing = GetById(id);
            if (existing != null)
            {
                existing.DoctorName = appointment.DoctorName;
                existing.AppointmentDate = appointment.AppointmentDate;
            }
        }

        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing != null)
            {
                _appointments.Remove(existing);
            }
        }
    }
}
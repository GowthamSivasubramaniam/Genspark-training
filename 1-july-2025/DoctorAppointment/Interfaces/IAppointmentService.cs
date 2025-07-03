using DoctorAppointment.Models;
using DoctorAppointment.Models.DTO;

namespace DoctorAppointment.Interfaces
{
    public interface IAppointmentService
    {
        public Task<Appointment> AddAppointment(AppointmentAddDto appointment);
        public Task<Appointment> GetAppointmentById(int id);
        public Task<Appointment> CancelAppointmentById(int id);
        

        
    }
}
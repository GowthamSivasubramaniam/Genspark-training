using cardioManagement.Models;
using cardioManagement.Repositories;
namespace cardioManagement.Services
{
    public class AppointmentService
    {
        public static AppointmentRepository _appointments = new AppointmentRepository();

        public int Add(Appointment appointment)
        {
            try
            {
                var item = _appointments.Add(appointment);
                return item.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return -1;
        }

        public List<Appointment> SearchAppointments(SearchModel searchModel)
        {
            var all = _appointments.GetAll();

            var results = all.AsQueryable();

            if (searchModel.Id != null)
            {
                results = results.Where(a => a.Id == searchModel.Id);
            }
            if (!string.IsNullOrWhiteSpace(searchModel.PatientName))
            {
                results = results.Where(a => a.PatientName.ToLower().Contains(searchModel.PatientName.ToLower()));
            }
            if (searchModel.PatientAge != null)
            {
                results = results.Where(a => a.PatientAge == searchModel.PatientAge);
            }
            if (searchModel.AppointmentDate != null)
            {
                results = results.Where(a => a.AppointmentDate.Date == searchModel.AppointmentDate.Value.Date);
            }
            if (!string.IsNullOrWhiteSpace(searchModel.Reason))
            {
                results = results.Where(a => a.Reason.ToLower().Contains(searchModel.Reason.ToLower()));
            }

            return results.ToList();
        }
    }
}

using Notify.Models;
using Notify.Models.DTO;

namespace Notify.Interfaces
{
    public interface IUserService
    {
         public Task<User> GetUserByEmail(string mail);
        // public Task<ICollection<DisplayDoctorDto>> GetDoctorsBySpeciality(string speciality);
        public Task<string> AddUser(UserAddDto dto);
        // public  Task<IEnumerable<DisplayDoctorDto>> GetDoctors();
    }
}
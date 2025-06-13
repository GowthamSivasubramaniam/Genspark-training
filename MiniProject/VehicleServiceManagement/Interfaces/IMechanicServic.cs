using VSM.DTO;
using VSM.Models;

namespace VSM.Interfaces
{
    public interface IMechanicService
    {
        Task<MechanicDisplayDto> AddMechanic(MechanicAddDto dto);

        Task<bool> DeleteMechanic(string email);

        Task<MechanicDisplayDto?> GetByEmail(string email);

        Task<IEnumerable<MechanicDisplayDto>> GetByName(string name);

        Task<IEnumerable<MechanicDisplayDto>> GetAll();

        Task<MechanicDisplayDto?> UpdateMechanic(string email, MechanicUpdateDto dto);
    }
}
using VSM.DTO;
using VSM.Models;

namespace VSM.Interfaces
{
    public interface IVehicleService
    {
        Task<VehicleDisplayDto> AddVehicle(VehicleAdd dto);

        Task<bool> DeleteVehicle( Guid id);

        Task<VehicleDisplayDto?> GetById(Guid Id);


        Task<IEnumerable<VehicleDisplayDto>> GetAll();

        Task<VehicleDisplayDto?> UpdateVehicleInfo(Guid id, VehicleAdd dto);
    }
}
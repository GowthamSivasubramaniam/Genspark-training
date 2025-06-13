using VSM.DTO;

namespace VSM.Interfaces
{
    public interface IServiceService
    {
        Task<ServiceDisplayDto> AddService(ServiceAddDto dto);
        Task<bool> SoftDeleteService(Guid serviceId);
        Task<ServiceDisplayDto?> GetById(Guid serviceId);
        Task<IEnumerable<ServiceDisplayDto>> GetByVehicleId(Guid vehicleId);
        Task<ServiceDisplayDto> UpdateServiceCategories(Guid serviceId, List<string> categoryNames);
    }
}
using VSM.DTO;

namespace VSM.Interfaces
{
    public interface IServiceRecordService
    {
        Task<ServiceRecordDisplayDto> Add(ServiceRecordAddDto dto);
        Task<ServiceRecordDisplayDto> UpdateStatus(ServiceRecordStatusUpdateDto dto);
        Task<ServiceRecordDisplayDto?> Get(Guid serviceRecordId);
        Task<IEnumerable<ServiceRecordDisplayDto>> GetAll();
        Task<IEnumerable<ServiceRecordDisplayDto>> GetByCustomerId(Guid customerId);
        Task<IEnumerable<ServiceRecordDisplayDto>> GetByMechanicId(Guid mechanicId);
    }
}
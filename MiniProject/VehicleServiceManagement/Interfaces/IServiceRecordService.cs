using VSM.DTO;

namespace VSM.Interfaces
{
    public interface IServiceRecordService
    {
        Task<ServiceRecordDisplayDto> Add(ServiceRecordAddDto dto);
        Task<ServiceRecordDisplayDto> UpdateStatus(ServiceRecordStatusUpdateDto dto);
        Task<ServiceRecordDisplayDto?> Get(Guid serviceRecordId);
        Task<IEnumerable<ServiceRecordDisplayDto>> GetAll(int page, int pageSize, string? search = null);
        Task<IEnumerable<ServiceRecordDisplayDto>> GetByCustomerId(Guid customerId);
        Task<IEnumerable<ServiceRecordDisplayDto>> GetByMechanicId(Guid mechanicId);
    }
}
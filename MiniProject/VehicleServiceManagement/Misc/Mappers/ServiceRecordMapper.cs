using VSM.Models;
using VSM.DTO;

namespace VSM.Misc.Mappers
{
    public class ServiceRecordMapper
    {
        public ServiceRecord MapAddDto(ServiceRecordAddDto dto)
        {
            return new ServiceRecord
            {
                MechanicId = dto.MechanicId,
                CustomerID = dto.CustomerID,
                ServiceID = dto.ServiceID,
                BookingID = dto.BookingID,
                Status = "Active"
            };
        }

        public ServiceRecordDisplayDto MapToDisplayDto(ServiceRecord record)
        {
            return new ServiceRecordDisplayDto
            {
                ServiceRecordID = record.ServiceRecordID,
                MechanicId = record.MechanicId,
                CustomerID = record.CustomerID,
                ServiceID = record.ServiceID,
                BookingID = record.BookingID,
                Status = record.Status,
                
            };
        }

        public IEnumerable<ServiceRecordDisplayDto> MapToDisplayDtos(IEnumerable<ServiceRecord> records)
        {
            return records.Select(MapToDisplayDto);
        }
    }
}
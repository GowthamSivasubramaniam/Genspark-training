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

                // Customer details
                CustomerName = record.Customer?.Name ?? "",
                Customer_Email = record.Customer?.Email ?? "",
                Customer_PhoneNo = record.Customer?.Phone ?? "",


                MechanicName = record.Mechanic?.Name ?? "",
                Mechanic_Email = record.Mechanic?.Email ?? "",
                Mechanic_PhoneNo = record.Mechanic?.Phone ?? "",


                Description = record.Service?.Description ?? "",
                Categories = record.Service?.ServiceCategories?.Select(c => c.Name).ToArray() ?? new string[0],


                VehicleNo = record.Service?.Vehicle?.VehicleNo ?? ""
            };
}


        public IEnumerable<ServiceRecordDisplayDto> MapToDisplayDtos(IEnumerable<ServiceRecord> records)
        {
            return records.Select(MapToDisplayDto);
        }
    }
}
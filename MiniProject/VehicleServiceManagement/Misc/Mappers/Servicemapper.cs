using VSM.Models;
using VSM.DTO;

namespace VSM.Misc.Mappers
{
    public class ServiceMapper
    {
        public Service MapAddDto(ServiceAddDto dto, List<ServiceCategory> categories)
        {
            return new Service
            {
                VehicleID = dto.VehicleID,
                Description = dto.Description,
                ServiceCategories = categories
            };
        }

        public ServiceDisplayDto MapToDisplayDto(Service service)
        {
            return new ServiceDisplayDto
            {
                ServiceID = service.ServiceID,
                VehicleID = service.VehicleID,
                Description = service.Description,
                Categories = service.ServiceCategories.Select(c => c.Name).ToList(),
                IsDeleted = service.IsDeleted
            };
        }

        public IEnumerable<ServiceDisplayDto> MapToDisplayDtos(IEnumerable<Service> services)
        {
            return services.Select(MapToDisplayDto);
        }
    }
}
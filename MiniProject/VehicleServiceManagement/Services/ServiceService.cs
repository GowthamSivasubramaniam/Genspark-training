using VSM.Models;
using VSM.DTO;
using VSM.Interfaces;
using VSM.Misc.Mappers;
using System.Linq;

namespace VSM.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IRepository<Guid, Service> _serviceRepo;
        private readonly IRepository<Guid, ServiceCategory> _categoryRepo;
        private readonly IRepository<Guid, Vehicle> _vehicleRepo;
        private readonly ServiceMapper _mapper = new();

        public ServiceService(
            IRepository<Guid, Service> serviceRepo,
            IRepository<Guid, ServiceCategory> categoryRepo,
            IRepository<Guid, Vehicle> vehicleRepo)
        {
            _serviceRepo = serviceRepo;
            _categoryRepo = categoryRepo;
            _vehicleRepo = vehicleRepo;
        }

        public async Task<ServiceDisplayDto> AddService(ServiceAddDto dto)
        {
            var vehicle = await _vehicleRepo.Get(dto.VehicleID);
            Console.WriteLine(vehicle.VehicleID);
            if (vehicle == null || vehicle.IsDeleted)
                throw new Exception("Vehicle not found");

            var allCategories = await _categoryRepo.GetAll(1,100);
            var categories = allCategories
                .Where(c => dto.CategoryNames.Contains(c.Name, StringComparer.OrdinalIgnoreCase))
                .ToList();

            if (categories.Count != dto.CategoryNames.Count)
                throw new Exception("One or more categories not found");

            var service = _mapper.MapAddDto(dto, categories);
            
            var added = await _serviceRepo.Add(service) ?? throw new Exception("Unable to add service");
            return _mapper.MapToDisplayDto(added);
        }

        public async Task<bool> SoftDeleteService(Guid serviceId)
        {
            var service = await _serviceRepo.Get(serviceId);
            if (service == null || service.IsDeleted)
                throw new Exception("Service not found");
            service.IsDeleted = true;
            await _serviceRepo.Update(service.ServiceID, service);
            return true;
        }

        public async Task<ServiceDisplayDto?> GetById(Guid serviceId)
        {
            var service = await _serviceRepo.Get(serviceId);
            if (service == null || service.IsDeleted)
                 throw new Exception("Service not found");
            return _mapper.MapToDisplayDto(service);
        }

        public async Task<IEnumerable<ServiceDisplayDto>> GetByVehicleId(Guid vehicleId)
        {
            var services = (await _serviceRepo.GetAll(1,100))
                .Where(s => s.VehicleID == vehicleId && !s.IsDeleted);
            if(services.Count()==0)
             throw new Exception("No Services Found");
            return _mapper.MapToDisplayDtos(services);
        }

        public async Task<ServiceDisplayDto> UpdateServiceCategories(Guid serviceId, List<string> categoryNames)
        {
             var service = await _serviceRepo.Get(serviceId);
    if (service == null || service.IsDeleted)
        throw new Exception("Service not found");

    var allCategories = await _categoryRepo.GetAll(1, 100);
    var matchedCategories = allCategories
        .Where(c => categoryNames.Contains(c.Name, StringComparer.OrdinalIgnoreCase))
        .ToList();

    if (matchedCategories.Count != categoryNames.Count)
        throw new Exception("One or more categories not found");

    // Filter out duplicates that already exist in the service
    var existingCategoryNames = service.ServiceCategories
        .Select(sc => sc.Name)
        .ToHashSet(StringComparer.OrdinalIgnoreCase);

    var newCategories = matchedCategories
        .Where(c => !existingCategoryNames.Contains(c.Name))
        .ToList();

    if (!newCategories.Any())
        throw new Exception("All categories already exist. No new categories to add.");

    
    foreach (var cat in newCategories)
    {
        service.ServiceCategories.Add(cat);
    }

    var updated = await _serviceRepo.Update(service.ServiceID, service) 
                  ?? throw new Exception("Update failed");

    return _mapper.MapToDisplayDto(updated);
        }
    }
}
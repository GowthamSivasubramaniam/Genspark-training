using VSM.Interfaces;
using VSM.Models;
using VSM.DTO;
using VSM.Misc.Mappers;

namespace VSM.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IRepository<Guid, Vehicle> _vehicleRepo;
        private readonly VehicleMapper _mapper;

        public VehicleService(IRepository<Guid, Vehicle> vehicleRepo)
        {
            _vehicleRepo = vehicleRepo;
            _mapper = new VehicleMapper();
        }

        public async Task<VehicleDisplayDto> AddVehicle(VehicleAdd dto)
        {
            var vehicleNo = dto.VehicleNo.Trim().ToUpperInvariant();
            var vehicleType = dto.VehicleType.Trim().ToUpperInvariant();
            var manufacturer = dto.VechicleManufacturer.Trim().ToUpperInvariant();
            var model = dto.VehicleModel.Trim().ToUpperInvariant();


            var allVehicles = await _vehicleRepo.GetAll(1, 100);


            var existing = allVehicles.FirstOrDefault(v =>
                v.VehicleNo.Trim().ToUpperInvariant() == vehicleNo &&
                v.VehicleType.Trim().ToUpperInvariant() == vehicleType &&
                v.VechicleManufacturer.Trim().ToUpperInvariant() == manufacturer &&
                v.VehicleModel.Trim().ToUpperInvariant() == model &&
                !v.IsDeleted
            );

            if (existing != null)
            {
                throw new Exception("Vehicle with the same details already exists.");
            }
            var vehicle = _mapper.MapVehicle(dto);
            var added = await _vehicleRepo.Add(vehicle) ?? throw new Exception("Unable to add vehicle");
            return _mapper.MapToDisplayDto(added);
        }

        public async Task<bool> DeleteVehicle(Guid id)
        {
            var all = await _vehicleRepo.GetAll(1, 100);
            var vehicle = all.FirstOrDefault(v => v.VehicleID == id);
            if (vehicle == null || vehicle.IsDeleted)
            {
                throw new Exception("Vehicle not Found");
            }
            vehicle.IsDeleted = true;
            var updated = await _vehicleRepo.Update(vehicle.VehicleID, vehicle);
            return true;
        }

        public async Task<VehicleDisplayDto?> GetById(Guid Id)
        {
            var vehicle = await _vehicleRepo.Get(Id);
            if (vehicle == null || vehicle.IsDeleted)
            {
                throw new Exception("Vehicle not Found");
            }
            return vehicle == null ? null : _mapper.MapToDisplayDto(vehicle);
        }

        public async Task<IEnumerable<VehicleDisplayDto>> GetAll()
        {
            var vehicles = await _vehicleRepo.GetAll(1, 100);
            var filteredvehicles = vehicles.Where(v => v.IsDeleted == false);
            if (filteredvehicles.Count() == 0)
                throw new Exception("Vehicles not Found");
            return _mapper.MapToDisplayDtos(filteredvehicles);
        }

        public async Task<VehicleDisplayDto?> UpdateVehicleInfo(Guid id, VehicleAdd dto)
        {
            var all = await _vehicleRepo.GetAll(1, 100);
            var vehicle = all.FirstOrDefault(v => v.VehicleID == id);
            if (vehicle == null || vehicle.IsDeleted)
            {
                throw new Exception("Vehicle not Found");
            }

            vehicle.VehicleModel = dto.VehicleModel;
            vehicle.VehicleType = dto.VehicleType;
            vehicle.VechicleManufacturer = dto.VechicleManufacturer;

            var updated = await _vehicleRepo.Update(vehicle.VehicleID, vehicle);
            return updated == null ? null : _mapper.MapToDisplayDto(updated);
        }
    }
}

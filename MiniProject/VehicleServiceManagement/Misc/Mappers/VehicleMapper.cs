using VSM.DTO;
using VSM.Models;

namespace VSM.Misc.Mappers
{
    public class VehicleMapper
    {
        public Vehicle MapVehicle(VehicleAdd dto)
        {
            return new Vehicle
            {
                VehicleNo = dto.VehicleNo,
                VehicleType = dto.VehicleType,
                VechicleManufacturer = dto.VechicleManufacturer,
                VehicleModel = dto.VehicleModel
            };
        }

        public VehicleDisplayDto MapToDisplayDto(Vehicle vehicle)
        {
            return new VehicleDisplayDto
            {
                VehicleID = vehicle.VehicleID,
                VehicleNo = vehicle.VehicleNo,
                VehicleType = vehicle.VehicleType,
                VechicleManufacturer = vehicle.VechicleManufacturer,
                VehicleModel = vehicle.VehicleModel
            };
        }

        public IEnumerable<VehicleDisplayDto> MapToDisplayDtos(IEnumerable<Vehicle> vehicles)
        {
            return vehicles.Select(MapToDisplayDto);
        }
    }
}

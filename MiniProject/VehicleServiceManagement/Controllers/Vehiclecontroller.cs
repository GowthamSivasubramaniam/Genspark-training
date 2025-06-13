
using Microsoft.AspNetCore.Mvc;
using VSM.Interfaces;
using VSM.DTO;
using VSM.Misc;
using Microsoft.AspNetCore.Authorization;
namespace VSM.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IFileLogger _logger;

        public VehicleController(IVehicleService vehicleService, IFileLogger logger)
        {
            _vehicleService = vehicleService;
            _logger = logger;
        }
        [Authorize(Roles = "Admin,Mechanic")]
        [HttpPost]
        public async Task<ActionResult<VehicleDisplayDto>> AddVehicle([FromBody] VehicleAdd dto)
        {
            try
            {
                var result = await _vehicleService.AddVehicle(dto);
                _logger.LogData($"Vehicle Added {result.VehicleID}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Adding Vehicle", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Mechanic")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVehicle(Guid id)
        {
            try
            {
                await _vehicleService.DeleteVehicle(id);
                _logger.LogData($"Vehicle Deleted {id}");
                return Ok("Deleted Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Deleting Vehicle", ex);
                return NotFound(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Mechanic")]
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDisplayDto>> GetById(Guid id)
        {
            try
            {
                var result = await _vehicleService.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Getting Vehicle By Id", ex);
                return NotFound(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Mechanic")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleDisplayDto>>> GetAll()
        {
            try
            {
                var result = await _vehicleService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Getting All Vehicles", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Mechanic")]
        [HttpPut("{id}")]
        public async Task<ActionResult<VehicleDisplayDto>> UpdateVehicleInfo(Guid id, [FromBody] VehicleAdd dto)
        {
            try
            {
                var result = await _vehicleService.UpdateVehicleInfo(id, dto);
                _logger.LogData($"Vehicle Updated {id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Updating Vehicle", ex);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
       
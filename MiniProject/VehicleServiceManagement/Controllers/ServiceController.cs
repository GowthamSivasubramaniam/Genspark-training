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
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        private readonly IFileLogger _logger;

        public ServiceController(IServiceService serviceService, IFileLogger logger)
        {
            _serviceService = serviceService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Mechanic")]
        public async Task<ActionResult<ServiceDisplayDto>> AddService([FromBody] ServiceAddDto dto)
        {
            try
            {
                var result = await _serviceService.AddService(dto);
                _logger.LogData($"Service Added {result.ServiceID}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Adding Service", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Mechanic")]
        [HttpDelete("{serviceId}")]
        public async Task<ActionResult> SoftDeleteService(Guid serviceId)
        {
            try
            {
                await _serviceService.SoftDeleteService(serviceId);
                _logger.LogData($"Service Soft Deleted {serviceId}");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Soft Deleting Service", ex);
                return NotFound(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Mechanic")]
        [HttpGet("{serviceId}")]
        public async Task<ActionResult<ServiceDisplayDto>> GetById(Guid serviceId)
        {
            try
            {
                var result = await _serviceService.GetById(serviceId);
                if (result == null)
                    return NotFound(new { message = "Service not found" });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Getting Service By Id", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Mechanic")]
        [HttpGet("vehicle/{vehicleId}")]
        public async Task<ActionResult<IEnumerable<ServiceDisplayDto>>> GetByVehicleId(Guid vehicleId)
        {
            try
            {
                var result = await _serviceService.GetByVehicleId(vehicleId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Getting Services By Vehicle Id", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Mechanic")]
        [HttpPut("{serviceId}/categories")]
        public async Task<ActionResult<ServiceDisplayDto>> UpdateServiceCategories(Guid serviceId, [FromBody] ServiceCategoryUpdateDto dto)
        {
            try
            {
                var result = await _serviceService.UpdateServiceCategories(serviceId, dto.CategoryNames);
                _logger.LogData($"Service Categories Updated for {serviceId}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Updating Service Categories", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
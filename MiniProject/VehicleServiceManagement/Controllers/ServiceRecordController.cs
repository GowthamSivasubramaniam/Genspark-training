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
    public class ServiceRecordController : ControllerBase
    {
        private readonly IServiceRecordService _serviceRecordService;
        private readonly IFileLogger _logger;

        public ServiceRecordController(IServiceRecordService serviceRecordService, IFileLogger logger)
        {
            _serviceRecordService = serviceRecordService;
            _logger = logger;
        }
         [Authorize(Roles = "Admin,Mechanic")]
        [HttpPost]
        public async Task<ActionResult<ServiceRecordDisplayDto>> Add([FromBody] ServiceRecordAddDto dto)
        {
            try
            {
                var result = await _serviceRecordService.Add(dto);
                _logger.LogData($"ServiceRecord Added {result.ServiceRecordID}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Adding ServiceRecord", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Mechanic")]
        [HttpPut("status")]
        public async Task<ActionResult<ServiceRecordDisplayDto>> UpdateStatus([FromBody] ServiceRecordStatusUpdateDto dto)
        {
            try
            {
                var result = await _serviceRecordService.UpdateStatus(dto);
                _logger.LogData($"ServiceRecord Status Updated {dto.ServiceRecordID}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Updating ServiceRecord Status", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Mechanic")]
        [HttpGet("{serviceRecordId}")]
        public async Task<ActionResult<ServiceRecordDisplayDto>> Get(Guid serviceRecordId)
        {
            try
            {
                var result = await _serviceRecordService.Get(serviceRecordId);
                if (result == null)
                    return NotFound(new { message = "Service record not found" });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Getting ServiceRecord By Id", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Mechanic")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceRecordDisplayDto>>> GetAll()
        {
            try
            {
                var result = await _serviceRecordService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Getting All ServiceRecords", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Mechanic,Customer")]
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<ServiceRecordDisplayDto>>> GetByCustomerId(Guid customerId)
        {
            try
            {
                var result = await _serviceRecordService.GetByCustomerId(customerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Getting ServiceRecords By CustomerId", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Mechanic")]
        [HttpGet("mechanic/{mechanicId}")]
        public async Task<ActionResult<IEnumerable<ServiceRecordDisplayDto>>> GetByMechanicId(Guid mechanicId)
        {
            try
            {
                var result = await _serviceRecordService.GetByMechanicId(mechanicId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Getting ServiceRecords By MechanicId", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
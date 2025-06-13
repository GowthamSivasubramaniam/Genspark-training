using Microsoft.AspNetCore.Mvc;
using VSM.DTO;
using VSM.Interfaces;
using VSM.Models;
using VSM.Misc;
using Microsoft.AspNetCore.Authorization;

namespace VSM.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MechanicController : ControllerBase
    {
        private readonly IMechanicService _mechanicService;
        private readonly IFileLogger _logger;

        public MechanicController(IMechanicService mechanicService, IFileLogger logger)
        {
            _mechanicService = mechanicService;
            _logger = logger;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<ActionResult<MechanicDisplayDto>> RegisterMechanic([FromBody] MechanicAddDto dto)
        {
            try
            {
                var mechanic = await _mechanicService.AddMechanic(dto);
                _logger.LogData($"Mechanic Registered {mechanic.Email}");
                return Ok(mechanic);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Registering Mechanic", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{email}")]
        public async Task<ActionResult> DeleteMechanic(string email)
        {
            try
            {
                var result = await _mechanicService.DeleteMechanic(email);
                _logger.LogData($"Mechanic Deleted {email}");
                return result ? Ok("Deleted Successfully"): NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Deleting Mechanic", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Mechanic")]
        [HttpGet("email/{email}")]
        public async Task<ActionResult<MechanicDisplayDto>> GetByEmail(string email)
        {
            try
            {
                var mechanic = await _mechanicService.GetByEmail(email);
                return Ok(mechanic);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Getting Mechanic By Email", ex);
                return NotFound(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("name/{name}")]
        public async Task<ActionResult<IEnumerable<MechanicDisplayDto>>> GetByName(string name)
        {
            try
            {
                var mechanics = await _mechanicService.GetByName(name);
                return Ok(mechanics);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Getting Mechanics By Name", ex);
                return NotFound(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MechanicDisplayDto>>> GetAll()
        {
            try
            {
                var mechanics = await _mechanicService.GetAll();
                return Ok(mechanics);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Getting All Mechanics", ex);
                return NotFound(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Mechanic")]
        [HttpPut("{email}")]
        public async Task<ActionResult<MechanicDisplayDto>> UpdateMechanic(string email, [FromBody] MechanicUpdateDto dto)
        {
            try
            {
                var updated = await _mechanicService.UpdateMechanic(email, dto);
                _logger.LogData($"Mechanic Updated {email}");
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Updating Mechanic", ex);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
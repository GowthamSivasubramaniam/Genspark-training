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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IFileLogger _logger;

        public CustomerController(ICustomerService customerService, IFileLogger logger)
        {
            _customerService = customerService;
            _logger = logger;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<CustomerDisplayDto>> RegisterCustomer([FromBody] CustomerAddDto dto)
        {
            try
            {
                var customer = await _customerService.AddCustomer(dto);
                _logger.LogData($"Customer Registered {customer.Email}");
                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Registering Customer", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Customer")]
        [HttpDelete("{email}")]
        public async Task<ActionResult> DeleteCustomer(string email)
        {
            try
            {
                var result = await _customerService.DeleteCustomer(email);
                _logger.LogData($"Customer Deleted {email}");
                return result ? Ok("Deleted Successfully") : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Deleting Customer", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Customer")]
        [HttpGet("email/{email}")]
        public async Task<ActionResult<CustomerDisplayDto>> GetByEmail(string email)
        {
            try
            {
                var customer = await _customerService.GetByEmail(email);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Getting Customer By Email", ex);
                return NotFound(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Mechanic")]
        [HttpGet("name/{name}")]
        public async Task<ActionResult<IEnumerable<CustomerDisplayDto>>> GetByName(string name)
        {
            try
            {
                var customers = await _customerService.GetByName(name);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Getting Customers By Name", ex);
                return NotFound(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Mechanic")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDisplayDto>>> GetAll()
        {
            try
            {
                var customers = await _customerService.GetAll();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Getting All Customers", ex);
                return NotFound(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,Customer")]
        [HttpPut("{email}")]
        public async Task<ActionResult<CustomerDisplayDto>> UpdateCustomer(string email, [FromBody] CustomerUpdateDto dto)
        {
            try
            {
                var updated = await _customerService.UpdateCustomer(email, dto);
                _logger.LogData($"Customer Updated {email}");
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Updating Customer", ex);
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
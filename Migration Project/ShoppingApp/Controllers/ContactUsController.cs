using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Models;
using ShoppingApp.Services;
using ShoppingApp.Services.Interfaces;

namespace ShoppingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactUController : ControllerBase
    {
        private readonly IContactUService _service;

        public ContactUController(IContactUService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactU>>> GetAll()
        {
            var contacts = await _service.GetAllAsync();
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactU>> GetById(int id)
        {
            var contact = await _service.GetByIdAsync(id);
            if (contact == null)
                return NotFound();

            return Ok(contact);
        }

        [HttpPost]
        public async Task<ActionResult<ContactU>> Create([FromBody] ContactU contact)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.CreateAsync(contact);
            return CreatedAtAction(nameof(GetById), new { id = contact.Id }, contact);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}

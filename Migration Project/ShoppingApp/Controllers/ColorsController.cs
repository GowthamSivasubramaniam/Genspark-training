// Controllers/ColorController.cs
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Dtos;
using ShoppingApp.Models;
using ShoppingApp.Services.Interfaces;

namespace ShoppingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ColorController : ControllerBase
    {
        private readonly IColorService _service;

        public ColorController(IColorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Color>>> GetAll()
        {
            var colors = await _service.GetAllAsync();
            return Ok(colors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Color>> GetById(int id)
        {
            var color = await _service.GetByIdAsync(id);
            if (color == null) return NotFound();
            return Ok(color);
        }

        [HttpPost]
        public async Task<ActionResult<Color>> Create(ColorDto color)
        {
            
            var created = await _service.CreateAsync(color);
            return CreatedAtAction(nameof(GetById), new { id = created.ColorId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ColorDto color)
        {
           

            var updated = await _service.UpdateAsync(id,color);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}

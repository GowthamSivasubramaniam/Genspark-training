namespace ShoppingApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ShoppingApp.Dtos;
    using ShoppingApp.Models;
    using ShoppingApp.Services.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class ModelController : ControllerBase
    {
        private readonly IModelService _modelService;

        public ModelController(IModelService modelService)
        {
            _modelService = modelService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Model>>> GetAll()
        {
            return Ok(await _modelService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Model>> Get(int id)
        {
            var model = await _modelService.GetByIdAsync(id);
            if (model == null) return NotFound();
            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult<Model>> Create(ModelDto model)
        {
            var created = await _modelService.AddAsync(model);
            return CreatedAtAction(nameof(Get), new { id = created.ModelId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ModelDto model)
        {

            var model1 = new Model
            {
                Model1 = model.Name
            };
            var updated = await _modelService.UpdateAsync(model1);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _modelService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}

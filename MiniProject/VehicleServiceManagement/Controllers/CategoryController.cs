using Microsoft.AspNetCore.Mvc;
using VSM.Interfaces;
using VSM.Models;
using VSM.Misc;
using Microsoft.AspNetCore.Authorization;

namespace VSM.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IFileLogger _logger;

        public CategoryController(ICategoryService categoryService, IFileLogger logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }
         [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ServiceCategory>> AddCategory( [FromBody] string name ,float amt)
        {
            try
            {
                var result = await _categoryService.AddCategory(name,amt);
                _logger.LogData($"Category Added {result.CategoryID}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Adding Category", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(Guid id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);
                _logger.LogData($"Category Deleted {id}");
                return Ok("Successfully Deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Deleting Category", ex);
                return NotFound(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult> UpdateCategory(Guid id,float Amount)
        {
            try
            {
                await _categoryService.UpdateCategory(id,Amount);
                _logger.LogData($"Updated Deleted {id}");
                return Ok("Successfully Updated");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Deleting Category", ex);
                return NotFound(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
       public async Task<ActionResult<IEnumerable<ServiceCategory>>> GetAll()
        {
            try
            {
                var categories = await _categoryService.GetAllCategory();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Getting All Bookings", ex);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
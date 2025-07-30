// Controllers/NewsController.cs
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Models;
using ShoppingApp.Services.Interfaces;

namespace ShoppingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _service;

        public NewsController(INewsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 2)
        {
            var news = await _service.GetAllAsync(page, pageSize);
            return Ok(news);
        }
    }
}

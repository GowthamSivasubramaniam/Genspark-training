using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Dtos;
using ShoppingApp.Interfaces;
using ShoppingApp.Models;

namespace ShoppingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsManagementController : ControllerBase
    {
        private readonly INewsManagementService _service;

        public NewsManagementController(INewsManagementService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<News>> Get(int id)
        {
            var item = await _service.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<News>> Create([FromForm] NewsDto news)
        {
            var created = await _service.CreateAsync(news);
            return CreatedAtAction(nameof(Get), new { id = created.NewsId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, NewsDto news)
        {
            Console.WriteLine(news);
            var success = await _service.UpdateAsync(id,news);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpGet("export/csv")]
        public async Task<IActionResult> ExportCsv()
        {
            var file = await _service.ExportToCsvAsync();
            return File(file, "text/csv", $"News_{DateTime.Now:yyyyMMdd}.csv");
        }

        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportExcel()
        {
            var file = await _service.ExportToExcelAsync();
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"News_{DateTime.Now:yyyyMMdd}.xlsx");
        }
    }
}

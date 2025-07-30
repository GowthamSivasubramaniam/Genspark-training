using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Dtos;
using ShoppingApp.Models;
using ShoppingApp.Services.Interfaces;

namespace ShoppingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll(int page = 1, int pageSize = 5)
        {
            var orders = await _service.GetAllAsync(page, pageSize);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(int id)
        {
            var order = await _service.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create(OrderDto order)
        {
            var created = await _service.CreateAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = created.OrderID }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Order order)
        {
            if (id != order.OrderID)
                return BadRequest();

            var updated = await _service.UpdateAsync(order);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

       [HttpGet("export/excel")]
public IActionResult ExportExcel()
{
    var fileContents = _service.ExportOrdersToExcel();
    var fileName = $"OrderListing_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
    return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
}

[HttpGet("export/pdf")]
public IActionResult ExportPdf()
{
    var fileContents = _service.ExportOrdersToPdf();
    var fileName = $"OrderListing_{DateTime.Now:yyyyMMddHHmmss}.pdf";
    return File(fileContents, "application/pdf", fileName);
}
    }
}

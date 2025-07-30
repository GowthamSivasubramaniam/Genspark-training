using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Dtos;
using ShoppingApp.Models;
using ShoppingApp.Services.Interfaces;
using System.Threading.Tasks;

namespace ShoppingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailsService _service;

        public OrderDetailsController(IOrderDetailsService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
   
            var order = await _service.CreateOrderAsync(dto);
            return Ok(order);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            // Implement this if needed to return order details
            return NotFound();
        }
        [HttpGet]
        public async Task<ActionResult<GetOrderDto>> GetOrders()
        {
            var orders = await _service.GetOrders();
            return Ok(orders);
        }
    }
}

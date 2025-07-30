using ShoppingApp.Context;
using ShoppingApp.Dtos;
using ShoppingApp.Models;
using ShoppingApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ShoppingApp.Services
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private readonly ShoppingContext _context;

        public OrderDetailsService(ShoppingContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderDto dto)
        {
            var order = new Order
            {
                OrderName = dto.OrderName,
                OrderDate = dto.OrderDate,
                PaymentType = dto.PaymentType,
                Status = dto.Status,
                CustomerName = dto.CustomerName,
                CustomerPhone = dto.CustomerPhone,
                CustomerEmail = dto.CustomerEmail,
                CustomerAddress = dto.CustomerAddress
            };
            var order1 = _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var detailDto in dto.OrderDetails)
            {
                var orderDetail = new OrderDetail
                {
                    OrderID = order.OrderID,
                    ProductID = detailDto.ProductID,
                    Quantity = detailDto.Quantity,
                    Price = detailDto.Price
                };
                _context.OrderDetails.Add(orderDetail);
            Console.WriteLine(orderDetail.ProductID +"hii");
            }


            await _context.SaveChangesAsync();

            
            await _context.Entry(order).Collection(o => o.OrderDetails).LoadAsync();

            return order;
        }

    public async Task<IEnumerable<GetOrderDto>> GetOrders()
{
    var orderDetailsGrouped = await _context.OrderDetails
        .Include(od => od.Product)
        .Include(od => od.Order)
        .GroupBy(od => od.OrderID)
        .Select(g => new GetOrderDto
        {
            OrderID = g.Key,
            OrderName = g.First().Order.OrderName,
            OrderDate = g.First().Order.OrderDate,
            PaymentType = g.First().Order.PaymentType,
            Status = g.First().Order.Status,
            CustomerName = g.First().Order.CustomerName,
            CustomerPhone = g.First().Order.CustomerPhone,
            CustomerEmail = g.First().Order.CustomerEmail,
            CustomerAddress = g.First().Order.CustomerAddress,
            OrderDetails = g.Select(od => new GetOrderDetailDto
            {
                ProductID = od.ProductID,
                Quantity = od.Quantity,
                Price = od.Price,
                ProductName = od.Product.ProductName
            }).ToList()
        })
        .ToListAsync();

    return orderDetailsGrouped;

}

    }
}

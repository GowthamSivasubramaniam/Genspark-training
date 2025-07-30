using ShoppingApp.Dtos;
using ShoppingApp.Models;
using System.Threading.Tasks;

namespace ShoppingApp.Services.Interfaces
{
    public interface IOrderDetailsService
    {
        Task<Order> CreateOrderAsync(CreateOrderDto dto);
        Task<IEnumerable<GetOrderDto>> GetOrders();
       
    }
}

using ShoppingApp.Dtos;
using ShoppingApp.Models;

namespace ShoppingApp.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllAsync(int page = 1, int pageSize = 5);
        Task<Order?> GetByIdAsync(int id);
        Task<Order> CreateAsync(OrderDto order);
        Task<bool> UpdateAsync(Order order);
        Task<bool> DeleteAsync(int id);
        byte[] ExportOrdersToExcel();
        byte[] ExportOrdersToPdf();
    }
}

    
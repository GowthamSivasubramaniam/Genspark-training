using ShoppingApp.Dtos;
using ShoppingApp.Models;

namespace ShoppingApp.Interfaces
{
    public interface INewsManagementService
    {
        Task<List<News>> GetAllAsync();
        Task<News?> GetByIdAsync(int id);
        Task<News> CreateAsync(NewsDto news);
        Task<bool> UpdateAsync(int id,NewsDto news);
        Task<bool> DeleteAsync(int id);
        Task<byte[]> ExportToCsvAsync();
        Task<byte[]> ExportToExcelAsync();
    }
}

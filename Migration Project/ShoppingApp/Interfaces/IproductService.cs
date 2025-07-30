using ShoppingApp.Dtos;
using ShoppingApp.Models;

namespace ShoppingApp.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDisplayDto>> GetAllAsync(int? categoryId = null, int page = 1, int pageSize = 10);
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(ProductDto product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
    }
}

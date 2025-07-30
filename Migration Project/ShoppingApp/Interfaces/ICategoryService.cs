
using ShoppingApp.Dtos;
using ShoppingApp.Models;

namespace ShoppingApp.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> CreateAsync(CategoryDto category);
        Task<Category?> UpdateAsync(Category category);
        Task<bool> DeleteAsync(int id);
    }
}

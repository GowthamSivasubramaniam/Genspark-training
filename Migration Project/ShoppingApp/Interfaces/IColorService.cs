// Services/Interfaces/IColorService.cs
using ShoppingApp.Dtos;
using ShoppingApp.Models;

namespace ShoppingApp.Services.Interfaces
{
    public interface IColorService
    {
        Task<IEnumerable<Color>> GetAllAsync();
        Task<Color?> GetByIdAsync(int id);
        Task<Color> CreateAsync(ColorDto color);
        Task<Color?> UpdateAsync(int id,ColorDto color);
        Task<bool> DeleteAsync(int id);
    }
}

// Services/Interfaces/INewsService.cs
using ShoppingApp.Models;

namespace ShoppingApp.Services.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<News>> GetAllAsync(int page = 1, int pageSize = 2);
    }
}

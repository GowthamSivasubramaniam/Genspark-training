// Services/NewsService.cs
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Context;
using ShoppingApp.Models;
using ShoppingApp.Services.Interfaces;

namespace ShoppingApp.Services
{
    public class NewsService : INewsService
    {
        private readonly ShoppingContext _context;

        public NewsService(ShoppingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<News>> GetAllAsync(int page = 1, int pageSize = 2)
        {
            return await _context.News
                .OrderByDescending(n => n.NewsId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}

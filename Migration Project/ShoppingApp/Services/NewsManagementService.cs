using Microsoft.EntityFrameworkCore;
using ShoppingApp.Context;
using ShoppingApp.Interfaces;
using ShoppingApp.Models;
using System.Text;
using System.Text.Json;
using System.Data;
using OfficeOpenXml;
using ShoppingApp.Dtos;

namespace ShoppingApp.Services
{
    public class NewsManagementService : INewsManagementService
    {
        private readonly ShoppingContext _context;

        public NewsManagementService(ShoppingContext context)
        {
            _context = context;
        }

        public async Task<List<News>> GetAllAsync() =>
            await _context.News.Include(n => n.User).ToListAsync();

        public async Task<News?> GetByIdAsync(int id) =>
            await _context.News.FindAsync(id);

        public async Task<News> CreateAsync(NewsDto news)
        {


 string? imagePath = null;
            if (news.Image != null && news.Image.Length > 0)
            {

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "NewsImages");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = $"{Guid.NewGuid()}_{news.Image.FileName}";
                imagePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await news.Image.CopyToAsync(stream);
                }
            }
            var newNews = new News
            {
                UserId = news.UserId,
                Title = news.Title,
                ShortDescription = news.ShortDescription,
                Image = imagePath,
                Content = news.Content
            };

            _context.News.Add(newNews);
            await _context.SaveChangesAsync();
            return newNews;
        }

        public async Task<bool> UpdateAsync(int id,NewsDto news)
        {
            var olditem = await GetByIdAsync(id);
           

            olditem.UserId = news.UserId;
            olditem.Title = news.Title;
            olditem.ShortDescription = news.ShortDescription;
            olditem.Content = news.Content;
          
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null) return false;

            _context.News.Remove(news);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<byte[]> ExportToCsvAsync()
        {
            var newsList = await _context.News.ToListAsync();
            var sb = new StringBuilder();
            sb.AppendLine("\"NewsId\",\"Title\",\"ShortDescription\",\"CreatedDate\",\"Status\"");
            foreach (var news in newsList)
            {
                sb.AppendLine($"\"{news.NewsId}\",\"{news.Title}\",\"{news.ShortDescription}\",\"{news.CreatedDate:yyyy-MM-dd}\",\"{news.Status}\"");
            }
            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public async Task<byte[]> ExportToExcelAsync()
        {
            var newsList = await _context.News.ToListAsync();
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("News");
            worksheet.Cells.LoadFromCollection(newsList, true);
            return await package.GetAsByteArrayAsync();
        }
    }
}

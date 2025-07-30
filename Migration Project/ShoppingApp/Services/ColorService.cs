// Services/ColorService.cs
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Context;
using ShoppingApp.Dtos;
using ShoppingApp.Models;
using ShoppingApp.Services.Interfaces;

namespace ShoppingApp.Services
{
    public class ColorService : IColorService
    {
        private readonly ShoppingContext _context;

        public ColorService(ShoppingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Color>> GetAllAsync()
        {
            return await _context.Colors.ToListAsync();
        }

        public async Task<Color?> GetByIdAsync(int id)
        {
            return await _context.Colors.FindAsync(id);
        }

        public async Task<Color> CreateAsync(ColorDto color)
        {
            var newColor = new Color { Color1 = color.Name };
            _context.Colors.Add(newColor);
            await _context.SaveChangesAsync();
            return newColor;
        }

        public async Task<Color?> UpdateAsync(int id,ColorDto color)
        {
            var existing = await _context.Colors.FindAsync(id);
            if (existing == null) throw new Exception("No color found");

            existing.Color1 = color.Name;
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var color = await _context.Colors.FindAsync(id);
            if (color == null) return false;

            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

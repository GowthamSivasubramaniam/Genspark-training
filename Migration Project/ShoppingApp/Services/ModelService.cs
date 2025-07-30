namespace ShoppingApp.Services
{
    using Microsoft.EntityFrameworkCore;
    using ShoppingApp.Context;
    using ShoppingApp.Dtos;
    using ShoppingApp.Models;
    using ShoppingApp.Services.Interfaces;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ModelService : IModelService
    {
        private readonly ShoppingContext _context;

        public ModelService(ShoppingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Model>> GetAllAsync()
        {
            return await _context.Models.Include(m => m.Products).ToListAsync();
        }

        public async Task<Model?> GetByIdAsync(int id)
        {
            return await _context.Models.Include(m => m.Products).FirstOrDefaultAsync(m => m.ModelId == id);
        }

        public async Task<Model> AddAsync(ModelDto model)
        {
            var newModel = new Model
            {
                Model1 = model.Name
            };
            _context.Models.Add(newModel);
            await _context.SaveChangesAsync();
            return newModel;
        }

        public async Task<bool> UpdateAsync(Model model)
        {
            var existing = await _context.Models.FindAsync(model.ModelId);
            if (existing == null) return false;

            existing.Model1 = model.Model1;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var model = await _context.Models.FindAsync(id);
            if (model == null) return false;

            _context.Models.Remove(model);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

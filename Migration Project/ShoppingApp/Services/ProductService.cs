using Microsoft.EntityFrameworkCore;
using ShoppingApp.Context;
using ShoppingApp.Dtos;
using ShoppingApp.Models;
using ShoppingApp.Services.Interfaces;

namespace ShoppingApp.Services
{
    public class ProductService : IProductService
    {
        private readonly ShoppingContext _context;

        public ProductService(ShoppingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDisplayDto>> GetAllAsync(int? categoryId = null, int page = 1, int pageSize = 10)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Model)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId);

            return await query
                .OrderByDescending(p => p.ProductId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDisplayDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ColorName = p.Color != null ? p.Color.Color1 : null,
                    Price = p.Price,
                    ImageUrl = p.Image,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.Name : null,
                    ModelName = p.Model != null ? p.Model.Model1 : null
                })
                .ToListAsync();
        }


        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Model)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<Product> CreateAsync(ProductDto product)
        {

            string? imagePath = null;
            if (product.Image != null && product.Image.Length > 0)
            {

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "ProductImages");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = $"{Guid.NewGuid()}_{product.Image.FileName}";
                imagePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await product.Image.CopyToAsync(stream);
                }
            }
            var newproduct = new Product
            {
                ProductName = product.ProductName,
                Image = imagePath ?? "",
                Price = product.Price,
                UserId = product.UserId,
                CategoryId = product.CategoryId,
                ColorId = product.ColorId,
                ModelId = product.ModelId,
                StorageId = product.StorageId,
                SellStartDate = product.SellStartDate,
                SellEndDate = product.SellEndDate,
                IsNew = product.IsNew
            };
            _context.Products.Add(newproduct);
            await _context.SaveChangesAsync();
            return newproduct;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ShoppingApp.Context;
using ShoppingApp.Models;
using ShoppingApp.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingApp.Services
{
    public class ContactUService : IContactUService
    {
        private readonly ShoppingContext _context;

        public ContactUService(ShoppingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ContactU>> GetAllAsync()
        {
            return await _context.ContactUs.ToListAsync();
        }

        public async Task<ContactU?> GetByIdAsync(int id)
        {
            return await _context.ContactUs.FindAsync(id);
        }

        public async Task CreateAsync(ContactU contact)
        {
            _context.ContactUs.Add(contact);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.ContactUs.FindAsync(id);
            if (entity != null)
            {
                _context.ContactUs.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}

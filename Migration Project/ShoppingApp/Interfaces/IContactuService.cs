using ShoppingApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingApp.Services.Interfaces
{
    public interface IContactUService
    {
        Task<IEnumerable<ContactU>> GetAllAsync();
        Task<ContactU?> GetByIdAsync(int id);
        Task CreateAsync(ContactU contact);
        Task DeleteAsync(int id);
    }
}

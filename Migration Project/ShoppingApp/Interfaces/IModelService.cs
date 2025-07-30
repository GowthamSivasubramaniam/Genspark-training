namespace ShoppingApp.Services.Interfaces
{
    using ShoppingApp.Dtos;
    using ShoppingApp.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IModelService
    {
        Task<IEnumerable<Model>> GetAllAsync();
        Task<Model?> GetByIdAsync(int id);
        Task<Model> AddAsync(ModelDto model);
        Task<bool> UpdateAsync(Model model);
        Task<bool> DeleteAsync(int id);
    }
}

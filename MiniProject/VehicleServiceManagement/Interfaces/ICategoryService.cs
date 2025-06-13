using VSM.Models;

namespace VSM.Interfaces
{
    public interface ICategoryService
    {
        Task<ServiceCategory> AddCategory(string name,float amt);
        Task<ServiceCategory> UpdateCategory(Guid id, float amt);
        Task<IEnumerable<ServiceCategory>> GetAllCategory();
        Task<bool> DeleteCategory(Guid id);
    }
}
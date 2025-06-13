using VSM.DTO;
using VSM.Models;

namespace VSM.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDisplayDto> AddCustomer(CustomerAddDto dto);
        Task<bool> DeleteCustomer(string email);
        Task<CustomerDisplayDto?> GetByEmail(string email);
        Task<IEnumerable<CustomerDisplayDto>> GetByName(string name);
        Task<IEnumerable<CustomerDisplayDto>> GetAll();
        Task<CustomerDisplayDto?> UpdateCustomer(string email, CustomerUpdateDto dto);
    }
}

using Microsoft.EntityFrameworkCore;
using VSM.DTO;
using VSM.Interfaces;
using VSM.Misc.Mappers;
using VSM.Models;

namespace VSM.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Guid, Customer> _repo;
        private readonly IRepository<string, User> _urepo;
        private readonly IEncryptionService _encryptionService;
        private readonly CustomerMapper _mapper;

        public CustomerService(IRepository<Guid, Customer> repo,
                                IRepository<string, User> urepo,
                                IEncryptionService encryptionService)
        {
            _repo = repo;
            _urepo = urepo;
            _encryptionService = encryptionService;
            _mapper = new CustomerMapper();
        }

        public async Task<CustomerDisplayDto> AddCustomer(CustomerAddDto dto)
        {
            var transaction = _repo.BeginTransaction();
            try
            {
                var customer = _mapper.MapCustomer(dto);

                var encryptedData = await _encryptionService.EncryptData(new EncryptModel { Data = dto.Password });

                var user = new User
                {
                    Email = dto.Email,
                    Role = "Customer",
                    IsActive = true,
                    Password = encryptedData.EncryptedData,
                    HashKey = encryptedData.HashKey
                };

                if (await _urepo.Get(dto.Email) != null)
                    throw new Exception("User already exists");

                await _urepo.Add(user);
                var addedCustomer = await _repo.Add(customer) ?? throw new Exception("Cannot add customer");

                await transaction.CommitAsync();
                return _mapper.MapCustomerToDisplayDto(addedCustomer);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteCustomer(string email)
        {
            var customers = await _repo.GetAll(1,100);
            var customer = customers.FirstOrDefault(c => c.Email == email && c.Status != "Deleted");
            if (customer == null) throw new Exception($"Customer with email '{email}' not found");

            var user = await _urepo.Get(email);
            if (user == null) throw new Exception($"User with email '{email}' not found");

            user.IsActive = false;
            customer.Status = "Deleted";

            await _urepo.Update(email, user);
            await _repo.Update(customer.CustomerID, customer);

            return true;
        }

        public async Task<CustomerDisplayDto?> GetByEmail(string email)
        {
            var customers = await _repo.GetAll(1,100);
            var customer = customers.FirstOrDefault(c => c.Email == email && c.Status != "Deleted");
            if (customer == null) throw new Exception($"Customer with email '{email}' not found");

            return _mapper.MapCustomerToDisplayDto(customer);
        }

        public async Task<IEnumerable<CustomerDisplayDto>> GetByName(string name)
        {
            var customers = await _repo.GetAll(1,100);
            var filtered = customers.Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase) && c.Status != "Deleted");
            if (!filtered.Any()) throw new Exception($"No customers found with name containing '{name}'");

            return _mapper.MapCustomerToDisplayDtos(filtered);
        }

        public async Task<IEnumerable<CustomerDisplayDto>> GetAll()
        {
            var customers = await _repo.GetAll(1,100);
            var filtered = customers.Where(c => c.Status != "Deleted");
            if (!filtered.Any()) throw new Exception("No customers found");

            return _mapper.MapCustomerToDisplayDtos(filtered);
        }

        public async Task<CustomerDisplayDto?> UpdateCustomer(string email, CustomerUpdateDto dto)
        {
            var customers = await _repo.GetAll(1,100);
            var customer = customers.FirstOrDefault(c => c.Email == email && c.Status != "Deleted");
            if (customer == null) throw new Exception($"Customer with email '{email}' not found");

            customer.Name = dto.Name;
            customer.Phone = dto.Phone;

            var updated = await _repo.Update(customer.CustomerID, customer);
            return _mapper.MapCustomerToDisplayDto(updated);
        }

       
    }
}
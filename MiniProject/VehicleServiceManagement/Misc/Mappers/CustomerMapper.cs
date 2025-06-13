using VSM.DTO;
using VSM.Models;

namespace VSM.Misc.Mappers
{
    public class CustomerMapper
    {
        public Customer MapCustomer(CustomerAddDto dto)
        {
            return new Customer
            {
                Name = dto.Name,
                Phone = dto.Phone,
                Email = dto.Email,
                Status = "Active"
            };
        }

        public CustomerDisplayDto MapCustomerToDisplayDto(Customer customer)
        {
            return new CustomerDisplayDto
            {
                CustomerId = customer.CustomerID,
                Name = customer.Name,
                Phone = customer.Phone,
                Email = customer.Email
            };
        }

        public IEnumerable<CustomerDisplayDto> MapCustomerToDisplayDtos(IEnumerable<Customer> customers)
        {
            return customers.Select(MapCustomerToDisplayDto);
        }
    }
}

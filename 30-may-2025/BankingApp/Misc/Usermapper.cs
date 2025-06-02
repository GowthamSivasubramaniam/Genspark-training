using BankingApp.DTO;
using BankingApp.Models;

namespace BankingApp.Mappers
{
    public class UserMapper
    {
        public User MapUserAddDtoToUser(UserAddDto dto)
        {
            return new User
            {
                Name = dto.Name,
                Phoneno = dto.Phoneno,
                Door_no = dto.Door_no,
                Area = dto.Area,
                City = dto.City,
                State = dto.State,
                Pincode = dto.Pincode,
                DOB = dto.DOB,
                PAN = dto.PAN,
                Accounts = new List<Account>()
            };
        }
    }
}

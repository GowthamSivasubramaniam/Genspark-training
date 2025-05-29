using BankingApp.DTO;
using BankingApp.Models;

namespace BankingApp.Interfaces
{
    public interface IUserService
    {
        public Task<User> AddUser(UserAddDto userDto);
        public Task<ShowUserDto> GetUserById(int id);
    }
}
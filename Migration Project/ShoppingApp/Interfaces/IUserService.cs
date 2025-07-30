using ShoppingApp.Dtos;
using ShoppingApp.Models;

namespace ShoppingApp.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(UserDto user);
        Task<User?> AuthenticateAsync(string username, string password);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
    }
}

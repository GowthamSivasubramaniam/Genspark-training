using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Context;
using ShoppingApp.Dtos;
using ShoppingApp.Models;

namespace ShoppingApp.Services
{
    public class UserService : IUserService
    {
        private readonly ShoppingContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(ShoppingContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<User> RegisterAsync(UserDto user)
        {
            var newuser = new User
            {
                Username = user.Username,
                Password = user.Password
            };
            newuser.Password = _passwordHasher.HashPassword(newuser, user.Password);
            _context.Users.Add(newuser);
            await _context.SaveChangesAsync();
            return newuser;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            return result == PasswordVerificationResult.Success ? user : null;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}

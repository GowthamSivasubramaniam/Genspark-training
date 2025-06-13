using VSM.DTO;
using System.Threading.Tasks;

namespace VSM.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserLoginResponse> Login(UserLoginRequest user);
        Task<UserLoginResponse> RefreshToken(string email, string refreshToken);
        Task Logout(string email);
    }
}
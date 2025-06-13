using VSM.Models;

namespace VSM.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
        string GenerateRefreshToken();
    }
}
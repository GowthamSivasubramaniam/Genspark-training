using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Notify.Contexts;
using Notify.Interfaces;

using Microsoft.IdentityModel.Tokens;

using Notify.Models;


namespace Notify.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _securityKey;
        public readonly NotifyContext _context;
        public TokenService(IConfiguration configuration, NotifyContext notifyContext)
        {
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Keys:JwtTokenKey"]));
            _context = notifyContext;
        }
        public async Task<string> GenerateToken(User user)
        {


            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Mail),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var creds = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
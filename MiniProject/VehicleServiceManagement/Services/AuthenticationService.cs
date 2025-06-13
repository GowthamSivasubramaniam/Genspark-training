using VSM.DTO;
using VSM.Interfaces;
using VSM.Models;
using Microsoft.Extensions.Logging;

namespace VSM.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly IEncryptionService _encryptionService;
        private readonly IRepository<string, User> _userRepository;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(ITokenService tokenService,
                                    IEncryptionService encryptionService,
                                    IRepository<string, User> userRepository,
                                    ILogger<AuthenticationService> logger)
        {
            _tokenService = tokenService;
            _encryptionService = encryptionService;
            _userRepository = userRepository;
            _logger = logger;
        }
        
        public async Task<UserLoginResponse> Login(UserLoginRequest user)
        {
            var dbUser = await _userRepository.Get(user.Email);
            if (dbUser == null)
            {
                _logger.LogCritical("User not found");
                throw new Exception("No such user");
            }
            var encryptedData = await _encryptionService.EncryptData(new EncryptModel
            {
                Data = user.Password,
                HashKey = dbUser.HashKey
            });
            for (int i = 0; i < encryptedData.EncryptedData.Length; i++)
            {
                if (encryptedData.EncryptedData[i] != dbUser.Password[i])
                {
                    _logger.LogError("Invalid login attempt");
                    throw new Exception("Invalid password");
                }
            }

            // If refresh token is null or expired, generate a new one
            if (string.IsNullOrEmpty(dbUser.RefreshToken) || dbUser.TTL < DateTime.Now)
            {
                dbUser.RefreshToken = _tokenService.GenerateRefreshToken();
                dbUser.TTL = DateTime.UtcNow.AddDays(7);
                await _userRepository.Update(dbUser.Email, dbUser);
            }

            var token = await _tokenService.GenerateToken(dbUser);

            return new UserLoginResponse
            {
                Email = user.Email,
                Token = token
            };
        }

        public async Task<UserLoginResponse> RefreshToken(string email, string refreshToken)
        {
            var dbUser = await _userRepository.Get(email);
            if (dbUser == null || dbUser.IsActive == false)
                throw new Exception("No such user");


            if (dbUser.TTL < DateTime.UtcNow)
                throw new Exception("Refresh token expired, please login again");

            var token = await _tokenService.GenerateToken(dbUser);

            return new UserLoginResponse
            {
                Email = dbUser.Email,
                Token = token
            };
        }

        public async Task Logout(string email)
        {
            var dbUser = await _userRepository.Get(email);
            if (dbUser == null)
                throw new Exception("No such user");

            dbUser.RefreshToken = null;
            dbUser.TTL = DateTime.MinValue;
            await _userRepository.Update(email, dbUser);
        }
    }
}
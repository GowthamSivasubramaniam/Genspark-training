using System.Net.Http.Headers;
using AutoMapper;



using Notify.Repositories;
using Microsoft.OpenApi.Validations;
using Notify.Interfaces;
using Notify.Models;
using Notify.Models.DTO;

namespace DoctorAppointment.Service
{

  
    public class UserService : IUserService
    {
       
        private readonly IRepository<string, User> _urepo;

         private readonly IEncryptionService _encryptionService;
    

        public UserService(IRepository<string, User> urepo,
                            IEncryptionService encryptionService
                            )
        {
            _encryptionService = encryptionService;
            _urepo = urepo;
          

        }

        

        public async Task<string> AddUser(UserAddDto dto)
        {

            var user = new User
            {
                Mail = dto.Mail
            };
            var encryptedData = await _encryptionService.EncryptData(new EncryptModel
            {
                Data = dto.Password
            });
            user.Password = encryptedData.EncryptedData;
            user.HashKey = encryptedData.HashKey;
            user.Role = dto.Role;
            user = await _urepo.Add(user);
            return "successfully added User";
        }

        public Task<User> GetUserByEmail(string mail)
        {
            throw new NotImplementedException();
        }
    }

   
}
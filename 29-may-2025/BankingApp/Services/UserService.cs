using BankingApp.Contexts;
using BankingApp.DTO;
using BankingApp.Interfaces;
using BankingApp.Mappers;
using BankingApp.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Services
{
    class UserService : IUserService
    {

        private readonly BankContext _bankContext;
        private readonly UserMapper _userMapper;

        public UserService(BankContext bankContext)
        {
            _bankContext = bankContext;
            _userMapper = new UserMapper();
        }



        public async Task<User> AddUser(UserAddDto userDto)
        {

            using var transaction = await _bankContext.Database.BeginTransactionAsync();
            try
            {
                var user = _userMapper.MapUserAddDtoToUser(userDto);
                var existinguser = _bankContext.users.FirstOrDefault(u => u.PAN == user.PAN);
                if (existinguser != null)
                {
                    throw new Exception("User Already exists");
                }

                var accountNumber = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 12);
               

                var account = AccountMapper.MapToAccount(user, accountNumber, userDto.AccountType);

                user.Accounts = new List<Account> { account };

                _bankContext.users.Add(user);
                await _bankContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return user;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<ShowUserDto> GetUserById(int id)
        {
            try
            {
                var user = await _bankContext.users.Include(u => u.Accounts).AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                    throw new Exception("User not found");
                var accounts = user.Accounts;
                if (accounts == null)
                    throw new Exception("No acctount associated with the user");
                List<ShowAccountInfoDto> acs = new List<ShowAccountInfoDto>();
                foreach (var item in accounts)
                {
                    ShowAccountInfoDto s = new ShowAccountInfoDto
                    {
                        AccountNo = item.AccountNo,
                        AccountType = item.AccountType,
                        Balance = item.Balance
                    };
                    acs.Add(s);
                }

                var show = new ShowUserDto
                {
                    Name = user.Name,
                    Phoneno = user.Phoneno,
                    Door_no = user.Door_no,
                    Area = user.Area,
                    City = user.City,
                    State = user.State,
                    Pincode = user.Pincode,
                    DOB = user.DOB,
                    PAN = user.PAN,
                    Accounts = acs
                };

                return show;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
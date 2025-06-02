using BankingApp.Contexts;
using BankingApp.DTO;
using BankingApp.Interfaces;
using BankingApp.Mappers;
using BankingApp.Models;
using Microsoft.EntityFrameworkCore;
namespace BankingApp.Services
{
    class AccounService : IAccountService
    {

        private readonly BankContext _bankContext;
        private readonly NewAccountMapper _acMapper;

        public AccounService(BankContext bankContext)
        {
            _bankContext = bankContext;
            _acMapper = new NewAccountMapper();
        }

        public async Task<Account> AddAccount(AccountAddDto account)
        {
            using var transaction = await _bankContext.Database.BeginTransactionAsync();
            try
            {
                var user =  _bankContext.users.FirstOrDefault(u => u.Id == account.UserId);
                if (user == null)
                {
                    throw new Exception($"User with {account.UserId} not found");
                }
                var accountNumber = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 12);
                var ac = _acMapper.MapAccountAddDtoToAccount(account, accountNumber);
                _bankContext.accounts.Add(ac);
                await _bankContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return ac;
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<ShowAccountInfoDto> GetAccountByACno(string accountNo)
        {
            try
            {
                var account = await _bankContext.accounts.FirstOrDefaultAsync(a => a.AccountNo == accountNo);

                if (account == null)
                {
                    throw new Exception("No Account with given No");
                }
                ShowAccountInfoDto s = new ShowAccountInfoDto
                {
                    AccountNo = account.AccountNo,
                    AccountType = account.AccountType,
                    Balance = account.Balance
                };
                if (s == null)
                {
                    throw new Exception("Sorry! Retry After Sometime");
                }
                return s;
            }
             catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
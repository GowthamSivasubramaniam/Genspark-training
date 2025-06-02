using BankingApp.DTO;
using BankingApp.Models;

namespace BankingApp.Mappers
{
    public class NewAccountMapper
    {
        public Account MapAccountAddDtoToAccount(AccountAddDto dto, string accountNo)
        {
            return new Account
            {
                AccountNo = accountNo,
                AccountType = dto.AccountType,
                Balance = dto.Balance,
                UserId = dto.UserId,
               
            };
        }
    }
}
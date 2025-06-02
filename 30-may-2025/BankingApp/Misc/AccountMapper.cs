using BankingApp.Models;

namespace BankingApp.Mappers
{
    public static class AccountMapper
    {
        public static Account MapToAccount(User user, string accountNo, string accountType)
        {
            return new Account
            {
                AccountNo = accountNo,
                AccountType = accountType,
                UserId = user.Id,
                user = user,
                Balance = 0 
            };
        }
    }
}

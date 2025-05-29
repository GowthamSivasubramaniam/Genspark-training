using BankingApp.DTO;
using BankingApp.Models;

public interface IAccountService
    {
        public Task<Account> AddAccount(AccountAddDto user);
       public Task<ShowAccountInfoDto> GetAccountByACno(string accountNo);
        
    }
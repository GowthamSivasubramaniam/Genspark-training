
using BankingApp.DTO;

namespace BankingApp.Interfaces
{
    public interface ITransactionservice
    {
        public Task<string> TransferFund(TransferDto dto);
        public Task<string> WithdrawFund(WithdrawDto dto);
    }
}
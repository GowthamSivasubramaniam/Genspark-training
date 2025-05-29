using BankingApp.DTO;
using BankingApp.Models;

namespace BankingApp.Mappers
{
    public class TransactionMapper
    {
        public Transaction MapWithdrawDtoToTransaction(WithdrawDto dto,string type)
        {
            return new Transaction
            {
                FromAccountNo = dto.AccountNo,
                Amount = dto.Amount,
                Type = type
            };
        }
        public Transaction MapTransferDtoToTransaction(TransferDto dto)
        {
            return new Transaction
            {
                ToAccountNo = dto.ToAccountNo,
                FromAccountNo = dto.FromAccountNo,
                Amount = dto.Amount,
                Type = "FundTransfer"
            };
        }
        
    }
}

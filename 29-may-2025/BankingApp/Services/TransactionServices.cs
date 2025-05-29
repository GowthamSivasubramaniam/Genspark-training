using BankingApp.Contexts;
using BankingApp.DTO;
using BankingApp.Interfaces;
using BankingApp.Mappers;
using BankingApp.Models;
using Microsoft.EntityFrameworkCore;
namespace BankingApp.Services
{
    class TransactionService : ITransactionservice
    {

        private readonly BankContext _bankContext;
        private readonly 
        public TransactionService(BankContext bankContext)
        {
            _bankContext = bankContext;
            _TcMapper = new TcMapper();
        }
        public Task<string> TransferFund(TransferDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<string> WithdrawFund(WithdrawDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
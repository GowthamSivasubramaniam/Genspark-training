using BankingApp.Contexts;
using BankingApp.DTO;
using BankingApp.Interfaces;
using BankingApp.Mappers;
using BankingApp.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace BankingApp.Services
{
    class TransactionService : ITransactionservice
    {

        private readonly BankContext _bankContext;
        private readonly TransactionMapper _TcMapper;
        public TransactionService(BankContext bankContext)
        {
            _bankContext = bankContext;
            _TcMapper = new TransactionMapper();
        }

        public async Task<string> DepositFund(WithdrawDto dto)
        {
            using var transaction = await _bankContext.Database.BeginTransactionAsync();
            try
            {
                var account = await _bankContext.accounts.FirstOrDefaultAsync(a => a.AccountNo == dto.AccountNo);
                if (account == null)
                {
                    throw new Exception("Invalid Account No");
                }
                if (dto.Amount <= 0)
                {
                    throw new Exception($"The Amount should be positive");
                }
                var withdrawl = _TcMapper.MapWithdrawDtoToTransaction(dto, "Deposit");

                account.Balance = account.Balance + withdrawl.Amount;
                await _bankContext.transactions.AddAsync(withdrawl);
                await _bankContext.SaveChangesAsync();
                await transaction.CommitAsync();
                var accountT = await _bankContext.accounts.FirstOrDefaultAsync(a => a.AccountNo == dto.AccountNo);

                return $"Successfully completed Deposit \n your balance is {accountT.Balance}";

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }

        }

        public async Task<string> TransferFund(TransferDto dto)
        {
            using var transaction = await _bankContext.Database.BeginTransactionAsync();
            try
            {
                var fromAccount = await _bankContext.accounts.FirstOrDefaultAsync(a => a.AccountNo == dto.FromAccountNo);
                var toAccount = await _bankContext.accounts.FirstOrDefaultAsync(a => a.AccountNo == dto.ToAccountNo);
                if (fromAccount == null)
                {
                    throw new Exception($"Invalid Account No : {fromAccount}");
                }
                if (toAccount == null)
                {

                    throw new Exception($"Invalid Account No : {toAccount}");
                }
                if (fromAccount.Balance < dto.Amount)
                {
                    throw new Exception($"The Amount is not available in your account \n Your balace: {fromAccount.Balance}");
                }
                var transfer = _TcMapper.MapTransferDtoToTransaction(dto);

                fromAccount.Balance = fromAccount.Balance - transfer.Amount;
                toAccount.Balance = toAccount.Balance + transfer.Amount;
                await _bankContext.transactions.AddAsync(transfer);
                await _bankContext.SaveChangesAsync();
                await transaction.CommitAsync();
                var accountT = await _bankContext.accounts.FirstOrDefaultAsync(a => a.AccountNo == dto.FromAccountNo);

                return $"Successfully completed Transfer \n your balance is {accountT.Balance}";

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<ICollection<ViewTransactionDTo>> ViewTransaction(string accountNo)
        {
            var account = await _bankContext.accounts.FirstOrDefaultAsync(a => a.AccountNo == accountNo);
            if (account == null)
            {
                throw new Exception("Invalid Account No");
            }
            var transactions =await _bankContext.showTransactions(accountNo);
            if (transactions.Count() == 0)
                throw new Exception("No transcations to show");
            // List<ViewTransactionDTo> transacts = new List<ViewTransactionDTo>();
            // foreach (var item in transactions)
            // {
            //     if (item.Type == "Deposit")
            //     {
            //         transacts.Add(
            //            new ViewTransactionDTo
            //            {
            //                TransactionType = item.Type,
            //                flowType = "Creadited",
            //                Amount = item.Amount
            //            }
            //         );
            //     }
            //     else if (item.Type == "Withdraw")
            //     {
            //         transacts.Add(
            //              new ViewTransactionDTo
            //              {
            //                  TransactionType = item.Type,
            //                  flowType = "Debited",
            //                  Amount = item.Amount
            //              }
            //         );
            //     }

            //     else if (item.Type == "FundTransfer")
            //     {
            //         if (item.ToAccountNo == accountNo)
            //         {
            //             transacts.Add(
            //              new ViewTransactionDTo
            //              {
            //                  TransactionType = item.Type,
            //                  flowType = "Credited",
            //                  Amount = item.Amount
            //              }
            //         );
            //         }
            //         else
            //         {
            //             transacts.Add(
            //              new ViewTransactionDTo
            //              {
            //                  TransactionType = item.Type,
            //                  flowType = "Debited",
            //                  Amount = item.Amount
            //              }
            //             );
            //         }
            //     }
            // }
            return transactions;

        }

        public async Task<string> WithdrawFund(WithdrawDto dto)
        {
            using var transaction = await _bankContext.Database.BeginTransactionAsync();
            try
            {
                var account = await _bankContext.accounts.FirstOrDefaultAsync(a => a.AccountNo == dto.AccountNo);
                if (account == null)
                {
                    throw new Exception("Invalid Account No");
                }
                if (account.Balance < dto.Amount)
                {
                    throw new Exception($"The Amount is not available in your account \n Your balace: {account.Balance}");
                }
                var withdrawl = _TcMapper.MapWithdrawDtoToTransaction(dto, "Withdraw");

                account.Balance = account.Balance - withdrawl.Amount;
                await _bankContext.transactions.AddAsync(withdrawl);
                await _bankContext.SaveChangesAsync();
                await transaction.CommitAsync();
                var accountT = await _bankContext.accounts.FirstOrDefaultAsync(a => a.AccountNo == dto.AccountNo);

                return $"Successfully completed withdrawl \n your balance is {accountT.Balance}";

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }

        }
    }
}
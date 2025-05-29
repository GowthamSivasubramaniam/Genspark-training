using System.Threading.Tasks;
using BankingApp.DTO;
using BankingApp.Interfaces;
using BankingApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionservice _ser;

        public TransactionController(ITransactionservice Service)
        {
            _ser = Service;
        }
        [HttpPost("withdraw")]
        public async Task<ActionResult<string>> withdraw(WithdrawDto dto)
        {
            try
            {
                return  Ok( await _ser.WithdrawFund(dto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("deposit")]
        public async Task<ActionResult<string>> Deposit(WithdrawDto dto)
        {
            try
            {
                return  Ok( await _ser.DepositFund(dto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       [HttpPost("transfer")]
        public async Task<ActionResult<string>> Transfer(TransferDto dto)
        {
            try
            {
                return  Ok( await _ser.TransferFund(dto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
         [HttpGet("showTransactions")]
        public async Task<ActionResult<string>> viewTransactionWithAcNo(string accountNo)
        {
            try
            {
                return Ok(await _ser.ViewTransaction(accountNo));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
using BankingApp.DTO;
using BankingApp.Interfaces;
using BankingApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _ser;

        public AccountController(IAccountService  Service)
        {
           _ser= Service;
        }

       
        [HttpPost]
        public async Task<ActionResult<User>> AddAccount( AccountAddDto dto)
        {
            try
            {
                var acc = await _ser.AddAccount(dto);
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

      
        [HttpGet("{accountno}")]
        public async Task<ActionResult<ShowAccountInfoDto>> GetUserById(string accountno)
        {
            try
            {
                var ac = await _ser.GetAccountByACno(accountno);
                return Ok(ac);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

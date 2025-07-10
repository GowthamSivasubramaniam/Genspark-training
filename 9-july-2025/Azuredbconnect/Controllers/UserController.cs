using AzConnect.Contexts;
using AzConnect.Models;
using Microsoft.AspNetCore.Mvc;


namespace AzConnect.Controllers
{
    [ApiController]
    [Route("/api/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly Context _context;
        public UserController(Context context)
        {
            _context = context;
        }

        [HttpPost]
     
public async Task<ActionResult<User>> AddUser(User _user)
{
    try
    {
        await _context.AddAsync(_user);  
        await _context.SaveChangesAsync();      
        return Ok(_user);                      
    }
    catch (Exception ex)
    {
        return BadRequest($"User cannot be added - {ex.Message}");
    }
}




    }
}
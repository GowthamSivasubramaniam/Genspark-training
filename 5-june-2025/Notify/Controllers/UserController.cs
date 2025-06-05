using Notify.Interfaces;
using Notify.Models;
using Notify.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Notify.Models;
using Notify.Models.DTO;
using Notify.Services;

namespace Notify.Controllers 
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _ser;

        public UserController(IUserService ser)
        {
            _ser = ser;
        }

        /*[HttpGet]
        public ActionResult<IEnumerable<Doctor>> GetDoctors()
        {
            return Ok(doctors);
        }*/
        [HttpPost]
        
        public async Task<ActionResult<string>> AddUser([FromBody] UserAddDto user)
        {
            try
            {

                var a = await _ser.AddUser(user);
                if (a != null)
                    return Created("", a);
                return BadRequest("Unable to process request at this moment");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
       


    }
}
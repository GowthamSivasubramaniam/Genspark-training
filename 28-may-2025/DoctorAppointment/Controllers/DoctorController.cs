using DoctorAppointment.Interfaces;
using DoctorAppointment.Models;
using DoctorAppointment.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointment.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorContoller : ControllerBase
    {
        private IDoctorService _ser;
        public DoctorContoller(IDoctorService ser)
        {
            _ser = ser;
        }

        [HttpPost]
        public async Task<ActionResult<Doctor>> Add(DoctorAddDto doctor)
        {
            try
            {
                var s = await _ser.AddDoctor(doctor);
                return Ok(s);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetAll()
        {
            try
            {
                var s = await _ser.GetDoctors();
                return Ok(s);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetByName(string name)
        {
            try
            {
                var s = await _ser.GetDoctByName(name);
                return Ok(s);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
         [HttpGet("by-speciality/{speciality}")]
         public async Task<ActionResult<IEnumerable<Doctor>>> GetDocBySpecality(string speciality)
        {
            try
            {
                var s = await _ser.GetDoctorsBySpeciality(speciality);
                return Ok(s);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        
        
        
       
    }
}
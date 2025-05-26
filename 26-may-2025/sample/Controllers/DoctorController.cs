using Microsoft.AspNetCore.Mvc;
using DocApp.Models;

namespace DocApp.Controllers
{
   [ApiController]
   [Route("/api/[controller]")]
   public class DoctorController : ControllerBase
   { 
        static List<Doctor> doctors = new List<Doctor>
        {
            new Doctor{id=1,Name="Gowtham"},
            new Doctor{id=2,Name="Siva"},
        };

       [HttpGet]
       [ProducesResponseType(StatusCodes.Status200OK)]
       [ProducesResponseType(StatusCodes.Status404NotFound)]
       public ActionResult<IEnumerable<Doctor>> GetDoctor()
       {
           return Ok(doctors);
       }

       [HttpPost]
       public ActionResult<Doctor> AddDoctor(Doctor doctor)
       {
            
           if (doctor == null)
           {
               return BadRequest("Doctor cannot be null");
           }
           doctors.Add(doctor);
           return Created("",doctor);
       }

       [HttpPut]
       public ActionResult<Doctor> UpdateDoctor (int id ,Doctor doctor)
       {
        Doctor _doctor = doctors.FirstOrDefault(d => d.id == id);
        if (_doctor == null)
        {
            return NotFound("Doctor not found");
        }
        _doctor.Name = doctor.Name;
        return Ok(_doctor);
       }

       [HttpDelete]
       public ActionResult<string> DeleteDoctor (int id)
       {
        
           Doctor _doctor = doctors.FirstOrDefault(d => d.id == id);
           if (_doctor == null)
           {
               return NotFound("Doctor not found");
           }
           doctors.Remove(_doctor);
           return Ok("Doctor deleted successfully");
       }


   }
} 
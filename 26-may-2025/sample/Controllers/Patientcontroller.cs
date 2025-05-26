using Microsoft.AspNetCore.Mvc;
using DocApp.Models;

namespace DocApp.Controllers
{
   [ApiController]
   [Route("/api/[controller]")]
   public class PatientController : ControllerBase
   { 
        static List<Patient> patients = new List<Patient>
        {
            new Patient{Id=1,Name="Gowtham",Phone_no = "1234567890" ,Diagnosis = "fever"},
            new Patient{Id=2,Name="Siva",Phone_no = "1234567340",Diagnosis = "fever"}
        };

       [HttpGet]
       [ProducesResponseType(StatusCodes.Status200OK)]
       [ProducesResponseType(StatusCodes.Status404NotFound)]
       public ActionResult<IEnumerable<Doctor>> GetPatient()
       {
           return Ok(patients);
       }

        [HttpPost]
        
       public ActionResult<Doctor> AddPatient(Patient patient)
        {

            if (patient == null)
            {
                return BadRequest("patient cannot be null");
            }
            patients.Add(patient);
            return Created("", patient);
        }

       [HttpPut]
       public ActionResult<Doctor> UpdatePatient (int id ,Patient patient)
       {
        Patient _patient = patients.FirstOrDefault(p => p.Id == id);
        if (_patient == null)
        {
            return NotFound("Patient not found");
        }
        _patient.Name = patient.Name;
        _patient.Phone_no = patient.Phone_no;
            _patient.Diagnosis = patient.Diagnosis;
        return Ok(_patient);
       }

       [HttpDelete]
       public ActionResult<string> DeletePatient (int id)
       {
        
           Patient _patient = patients.FirstOrDefault(d => d.Id == id);
           if (_patient == null)
           {
               return NotFound("Patient not found");
           }
           patients.Remove(_patient);
           return Ok("Patient deleted successfully");
       }


   }
} 
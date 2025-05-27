using DocApp.Models;
using DocApp.Services;
using Microsoft.AspNetCore.Mvc;
using DocApp.Services;
namespace DocApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly PatientService _service;

        public PatientController()
        {
            var repo = new Repositories.PatientRepository(); 
            _service = new PatientService(repo);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Patient>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Patient> Get(int id)
        {
            var patient = _service.Get(id);
            if (patient == null) return NotFound();
            return Ok(patient);
        }

        [HttpPost]
        public IActionResult Add(Patient patient)
        {

           var existingPatient = Get(patient.Id);
            if (existingPatient != null)
            {
                return BadRequest($"Existing Patient found with ID {patient.Id}");
            }
            _service.Add(patient);
            return Created("", patient);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Patient patient)
        {
            var existing = _service.Get(id);
            if (existing == null) return NotFound();
            _service.Update(id, patient);
            return Ok(patient);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _service.Get(id);
            if (existing == null) return NotFound();
            _service.Delete(id);
            return Ok("Deleted successfully");
        }
    }
}

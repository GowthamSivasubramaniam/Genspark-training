using DocApp.Models;
using DocApp.Services;
using Microsoft.AspNetCore.Mvc;
using DocApp.Services;
namespace DocApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly DoctorService _service;

        public DoctorController()
        {
            var repo = new Repositories.DoctorRepository(); 
            _service = new DoctorService(repo);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Doctor>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Doctor> Get(int id)
        {
            var doctor = _service.Get(id);
            if (doctor == null) return NotFound();
            return Ok(doctor);
        }

        [HttpPost]
        public IActionResult Add(Doctor doctor)
        {
            _service.Add(doctor);
            return Created("", doctor);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Doctor doctor)
        {
            var existing = _service.Get(id);
            if (existing == null) return NotFound();
            _service.Update(id, doctor);
            return Ok(doctor);
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

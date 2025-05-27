using DocApp.Models;
using DocApp.Services;
using Microsoft.AspNetCore.Mvc;
using DocApp.Services;
namespace DocApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _service;

        public AppointmentController()
        {
            var repo = new Repositories.AppointmentRepository(); 
            _service = new AppointmentService(repo);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Appointment>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Appointment> Get(int id)
        {
            var appointment = _service.Get(id);
            if (appointment == null) return NotFound();
            return Ok(appointment);
        }

        [HttpPost]
        public IActionResult Add(Appointment appointment)
        {
            _service.Add(appointment);
            return Created("", appointment);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Appointment appointment)
        {
            var existing = _service.Get(id);
            if (existing == null) return NotFound();
            _service.Update(id, appointment);
            return Ok(appointment);
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

using DoctorAppointment.Interfaces;
using DoctorAppointment.Models;
using DoctorAppointment.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointment.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _ser;

        public AppointmentController(IAppointmentService ser)
        {
            _ser = ser;
        }

        [HttpPost]
        public async Task<ActionResult<Appointment>> AddAppointment(AppointmentAddDto dto)
        {
            try
            {
                var newAppointment = await _ser.AddAppointment(dto);
                if (newAppointment != null)
                    return Created("", newAppointment);
                return BadRequest("Unable to process request at this moment");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            try
            {
                var appointment = await _ser.GetAppointmentById(id);
                return appointment;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("cancel/{id}")]
        [Authorize(Policy = "ExperienceOver5")]
        public async Task<ActionResult<Appointment>> CancelAppointment(int id)
        {
            try
            {
                var appointment = await _ser.CancelAppointmentById(id);
                return appointment;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file selected");

            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok("File uploaded successfully");
        }

        [HttpGet("download/{filename}")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", filename);

            if (!System.IO.File.Exists(path))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/octet-stream", filename);
        }



    }
}
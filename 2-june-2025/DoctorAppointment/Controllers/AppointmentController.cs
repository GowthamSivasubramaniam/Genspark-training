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

    }
}
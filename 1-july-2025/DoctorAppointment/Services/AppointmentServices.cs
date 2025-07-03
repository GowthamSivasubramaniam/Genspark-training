using System.Net.Http.Headers;
using System.Security.Claims;
using AutoMapper;
using DoctorAppointment.Contexts;
using DoctorAppointment.Interfaces;
using DoctorAppointment.Misc;
using DoctorAppointment.Models;
using DoctorAppointment.Models.DTO;
using DoctorAppointment.Repositories;
using DoctorAppointment.Services;
using FirstAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Validations;

namespace DoctorAppointment.Service
{

    public class AppointmentServices : IAppointmentService
    {
        private readonly IRepository<int, Appointment> _repo;
        private readonly IRepository<int, Doctor> _drepo;
        private readonly IRepository<int, Patient> _prepo;
        private readonly AuthorizationHandlerContext context;

        public AppointmentServices(IRepository<int, Appointment> repo,
        IRepository<int, Patient> prepo,
        IRepository<int, Doctor> drepo)
        {
            _repo = repo;
            _drepo = drepo;
            _prepo = prepo;
        }
        public async Task<Appointment> AddAppointment(AppointmentAddDto dto)
        {
            var doctor = await _drepo.Get(dto.DoctorId);
            if (doctor == null)
                throw new Exception("No Doctor with the given Id");
            var patient = await _prepo.Get(dto.PatientId);
            if (patient == null)
                throw new Exception("No Patient with the given Id");

            var appointment = new Appointment
            {
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                AppointmnetDateTime = dto.AppointmnetDateTime,
                Status = "Active"
            };

            var newappointment = await _repo.Add(appointment);
            if (newappointment == null)
            {
                throw new Exception("Appointment Cannot be Added");
            }
            return newappointment;


        }

        public async Task<Appointment> CancelAppointmentById(int id)
        {
            var appointment = await _repo.Get(id);
            var user =await _drepo.Get(appointment.DoctorId);
            string? nameIdentifier = ClaimTypes.NameIdentifier;

            if (user.Email != nameIdentifier)
             throw new Exception("Permission Denaid");
                if (appointment == null)
                    throw new Exception("No appointment with the given Id");
            appointment.Status = "Cancelled";
            var updatedAppointment = await _repo.Update(id, appointment);
            if (updatedAppointment == null)
                throw new Exception("cannot cancel the appointment");
            return updatedAppointment;

            
        }

        public async Task<Appointment> GetAppointmentById(int id)
        {
            var appointment = await _repo.Get(id);
            if (appointment == null)
                throw new Exception("No appointment with the given Id");
            return appointment;
        }
    }

}

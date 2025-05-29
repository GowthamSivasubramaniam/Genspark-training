using System.Text.Json.Serialization;
using DoctorAppointment.Contexts;
using DoctorAppointment.Interfaces;
using DoctorAppointment.Models;
using DoctorAppointment.Repositories;
using DoctorAppointment.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<ClinicContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // or null
        options.JsonSerializerOptions.WriteIndented = true;
    });


builder.Services.AddScoped<PatientRepo>();
builder.Services.AddScoped<SpecialityRepo>();
builder.Services.AddScoped<IPatientService, PatientServices>();
builder.Services.AddScoped<ISpecialityServices, SpecialityService>();
builder.Services.AddScoped<IRepository<int, Doctor>, DoctorRepo>();
builder.Services.AddScoped<IRepository<int, Speciality>, SpecialityRepo>();
builder.Services.AddScoped<IRepository<int, DoctorSpeciality>, DoctorSpecialityRepo>();
builder.Services.AddScoped<IDoctorService, DoctorService>();



var app = builder.Build();
if (app.Environment.IsDevelopment())
{   
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    }

app.MapControllers();
app.Run();



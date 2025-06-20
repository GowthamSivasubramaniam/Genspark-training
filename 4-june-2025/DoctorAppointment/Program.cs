using System.Text.Json.Serialization;
using DoctorAppointment.Contexts;
using DoctorAppointment.Interfaces;
using DoctorAppointment.Models;
using DoctorAppointment.Repositories;
using DoctorAppointment.Service;
using DoctorAppointment.Services;
using FirstAPI.Repositories;
using FirstAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DoctorAppointment.Misc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Clinic API", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
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
builder.Services.AddScoped<IRepository<int, Patient>, PatientRepo>();
builder.Services.AddScoped<IPatientService, PatientServices>();
builder.Services.AddScoped<ISpecialityServices, SpecialityService>();
builder.Services.AddScoped<IRepository<int, Doctor>, DoctorRepo>();
builder.Services.AddScoped<IRepository<int, Speciality>, SpecialityRepo>();
builder.Services.AddScoped<IRepository<int, DoctorSpeciality>, DoctorSpecialityRepo>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddTransient<IRepository<string, User>, UserRepository>();
builder.Services.AddTransient<IEncryptionService, EncryptionService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IRepository<int, Appointment>, AppointmentRepo>();

builder.Services.AddTransient<IAppointmentService, AppointmentServices>();

builder.Services.AddAutoMapper(typeof(User));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSwagger", policy =>
    {
        policy.WithOrigins("http://localhost:5297")  // Swagger UI origin
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Keys:JwtTokenKey"]))
                    };
                });

builder.Services.AddSingleton<IAuthorizationHandler, MinimumExperienceHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, MinimumExperiencePolicyProvider>();
builder.Logging.AddLog4Net();
builder.Services.AddScoped<CustomExceptionFilter>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{   
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    }
app.UseCors("AllowSwagger");
app.MapControllers();
app.Run();



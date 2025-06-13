using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VSM.Contexts;
using VSM.Interfaces;
using VSM.Models;
using VSM.Repositories;
using VSM.Services;
using QuestPDF;
using QuestPDF.Infrastructure;
using VSM.Misc;
using System.Threading.RateLimiting;
using Serilog;
using Microsoft.AspNetCore.Mvc;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddControllers();
builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 7176; 
});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1,0);
    options.ReportApiVersions = true; 
});

builder.Services.AddOpenApi();
QuestPDF.Settings.License = LicenseType.Community;
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
builder.Services.AddDbContext<VSMContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


#region Repository
builder.Services.AddTransient<IRepository<string, User>, UserRepository>();
builder.Services.AddTransient<IRepository<Guid, Mechanic>, MechanicRepository>();
builder.Services.AddTransient<IRepository<Guid, Customer>, CustomerRepository>();
builder.Services.AddTransient<IRepository<Guid, Booking>, BookingRepository>();
builder.Services.AddTransient<IRepository<Guid, Vehicle>, VehicleRepository>();
builder.Services.AddTransient<IRepository<Guid, ServiceCategory>, ServiceCategoriesRepository>();
builder.Services.AddTransient<IRepository<Guid, Service>, ServiceRepository>();
builder.Services.AddTransient<IRepository<Guid, ServiceRecord>, ServiceRecordRepository>();
builder.Services.AddTransient<IRepository<Guid, Bill>, BillRepository>();
#endregion


#region services
builder.Services.AddTransient<IEncryptionService, EncryptionService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IMechanicService, MechanicServices>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IBookingService, BookingService>();
builder.Services.AddTransient<IVehicleService, VehicleService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IServiceService, ServiceService>();
builder.Services.AddTransient<IServiceRecordService, ServiceRecordService>();
builder.Services.AddTransient<IBillService, BillService>();
#endregion

#region misc
builder.Services.AddSingleton<IFileLogger, FileLogger>();
#endregion


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
builder.Services.AddAuthorization();



builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("PerUserPolicy", context =>
    {
        var user = context.User.Identity?.Name ?? "anonymous";

        return RateLimitPartition.GetTokenBucketLimiter(user, _ => new TokenBucketRateLimiterOptions
        {
            TokenLimit = 1000, 
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0,
            ReplenishmentPeriod = TimeSpan.FromHours(1),
            TokensPerPeriod = 1000,
            AutoReplenishment = true
        });
    });
});
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("Logs/log-.log", 
                  rollingInterval: RollingInterval.Day,
                  retainedFileCountLimit: 7,
                  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRateLimiter();
app.UseHttpsRedirection();

app.MapControllers().RequireRateLimiting("PerUserPolicy");;

app.Run();

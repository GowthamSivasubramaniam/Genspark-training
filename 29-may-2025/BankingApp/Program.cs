using System.Text.Json.Serialization;
using BankingApp.Contexts;
using BankingApp.Interfaces;
using BankingApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<BankContext>(opts =>
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
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; 
        options.JsonSerializerOptions.WriteIndented = true;
    });
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAccountService, AccounService>();
builder.Services.AddTransient<ITransactionservice, TransactionService>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{   
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    }

app.MapControllers();
app.Run();



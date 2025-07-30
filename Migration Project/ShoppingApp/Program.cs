using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.FileProviders;
using ShoppingApp.Context;
using ShoppingApp.Interfaces;
using ShoppingApp.Services;
using ShoppingApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });
builder.Services.AddDbContext<ShoppingContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IModelService, ModelService>();
builder.Services.AddScoped<INewsService,NewsService>();
builder.Services.AddScoped<INewsManagementService,NewsManagementService>();
builder.Services.AddScoped<IContactUService,ContactUService>();
builder.Services.AddScoped<IOrderDetailsService,OrderDetailsService>();



builder.Services.AddSwaggerGen();


builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:4200") 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "ProductImages")),
    RequestPath = "/ProductImages"
});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "NewsImages")),
    RequestPath = "/NewsImages"
});
app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("AllowAll");
app.Run();


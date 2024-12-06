using Microsoft.EntityFrameworkCore;
using PromenaEmployeeManagement.Entities;
using PromenaEmployeeManagement.Controllers;
using PromenaEmployeeManagement.Repository.Service;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext with the connection string in appsettings.json
builder.Services.AddDbContext<PromenaEmployeeManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddHostedService<AttendanceBackgroundService>();

builder.Services.AddSingleton<S3Service>();

// Configure Swagger (for API documentation and testing)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use Swagger and Swagger UI in development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Run the application
app.Run();

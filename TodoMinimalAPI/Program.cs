using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoMinimalAPI.Data;
using TodoMinimalAPI.Endpoints;
using TodoMinimalAPI.Middleware;
using TodoMinimalAPI.Models;
using TodoMinimalAPI.Models.Requests;
using TodoMinimalAPI.Models.Responses;
using TodoMinimalAPI.Repository.Interface;
using TodoMinimalAPI.Repository.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//register DI services
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

//register db context as a service
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection"));
});

//enable cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontEnd",
        policy => policy.WithOrigins("http://127.0.0.1:5500")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

//register fluent validation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

//apply cors policy
app.UseCors("AllowFrontEnd");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//add exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

//adding endpoints using extension method
app.AddDepartmentEndpoints();
app.AddEmployeeEndpoints();

app.UseHttpsRedirection();

app.Run();


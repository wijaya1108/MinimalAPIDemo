using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoMinimalAPI.Data;
using TodoMinimalAPI.Models;
using TodoMinimalAPI.Models.Requests;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("api/departments", async (IDepartmentRepository _departmentRepository) =>
{
    var departmentsList = await _departmentRepository.GetDepartments();
    return Results.Ok(departmentsList);

}).Produces<IEnumerable<Department>>(200);

app.MapPost("api/departments", async (IDepartmentRepository _departmentRepository, [FromBody] DepartmentCreateRequest request) =>
{
    var result = await _departmentRepository.AddDepartment(request);
    return Results.Ok(result);
}).Accepts<DepartmentCreateRequest>("application/json").Produces<bool>(200);

app.UseHttpsRedirection();

app.Run();


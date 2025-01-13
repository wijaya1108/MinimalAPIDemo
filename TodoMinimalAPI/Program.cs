using FluentValidation;
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

//register fluent validation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

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

app.MapPost("api/departments", async (IDepartmentRepository _departmentRepository, IValidator<DepartmentCreateRequest> _validator, [FromBody] DepartmentCreateRequest request) =>
{
    var validationResult = await _validator.ValidateAsync(request);

    if (!validationResult.IsValid)
    {
        var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        return Results.BadRequest(errorMessages);
    }

    var result = await _departmentRepository.AddDepartment(request);
    return Results.Ok(result);

}).Accepts<DepartmentCreateRequest>("application/json").Produces<bool>(200);

app.MapGet("api/departments/{id:guid}", async (IDepartmentRepository _departmentRepository, Guid id) =>
{
    var result = await _departmentRepository.GetDepartmentById(id);

    if (result != null)
        return Results.Ok(result);

    return Results.NotFound($"Department with the requested id {id} does not exist!");
}).Produces<Department>(200).Produces(404);

app.UseHttpsRedirection();

app.Run();


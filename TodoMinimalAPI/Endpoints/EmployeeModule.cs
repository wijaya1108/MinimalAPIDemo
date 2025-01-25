using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TodoMinimalAPI.Models;
using TodoMinimalAPI.Models.Requests;
using TodoMinimalAPI.Models.Responses;
using TodoMinimalAPI.Repository.Interface;

namespace TodoMinimalAPI.Endpoints
{
    public static class EmployeeModule
    {
        public static void AddEmployeeEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/employee", async (IEmployeeRepository _empRepository,
                IValidator<EmployeeCreateRequest> _validator,
                [FromBody] EmployeeCreateRequest request) =>
            {
                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                    return Results.BadRequest(new
                    {
                        Errors = errorMessages
                    });
                }

                var result = await _empRepository.AddEmployee(request);

                return Results.Ok(result);

            });


            app.MapGet("api/employees", async (IEmployeeRepository _empRepository) =>
            {
                var result = await _empRepository.GetEmployees();

                return Results.Ok(result);

            }).Produces<IEnumerable<EmployeeResponse>>(200);


            app.MapGet("api/employees/{id:guid}", async (IEmployeeRepository _empRepository,
                Guid id) =>
            {
                var result = await _empRepository.GetEmployee(id);

                return Results.Ok(result);
            });
        }
    }
}

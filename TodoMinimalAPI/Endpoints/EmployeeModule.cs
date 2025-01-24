using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
                [FromBody] EmployeeCreateRequest request,
                APIResponse _apiResponse) =>
            {
                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                    _apiResponse.Success = false;
                    _apiResponse.Errors = errorMessages;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;

                    return Results.BadRequest(_apiResponse);
                }

                var result = await _empRepository.AddEmployee(request);

                _apiResponse.Result = result;

                return Results.Ok(_apiResponse);

            }).Produces<APIResponse>(201);


            app.MapGet("api/employees", async (IEmployeeRepository _empRepository,
                APIResponse _apiResponse) =>
            {

                var result = await _empRepository.GetEmployees();
                _apiResponse.Result = result;
                return Results.Ok(_apiResponse);

            }).Produces<APIResponse>(200);


            app.MapGet("api/employees/{id:guid}", async (IEmployeeRepository _empRepository,
                APIResponse _apiResponse,
                Guid id) =>
            {
                var result = await _empRepository.GetEmployee(id);
                _apiResponse.Result = result;

                return Results.Ok(_apiResponse);
            });
        }
    }
}

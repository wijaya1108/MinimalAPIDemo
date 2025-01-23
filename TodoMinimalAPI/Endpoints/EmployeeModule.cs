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
                [FromBody] EmployeeCreateRequest request) =>
            {
                APIResponse response = new APIResponse();
                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                    response.Success = false;
                    response.Errors = errorMessages;
                    response.StatusCode = HttpStatusCode.BadRequest;

                    return Results.BadRequest(response);
                }

                var result = await _empRepository.AddEmployee(request);

                return Results.Ok(response);
            }).Produces<APIResponse>(200);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using TodoMinimalAPI.Models.Requests;
using TodoMinimalAPI.Repository.Interface;

namespace TodoMinimalAPI.Endpoints
{
    public static class EmployeeModule
    {
        public static void AddEmployeeEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/employee", async (IEmployeeRepository _empRepository, [FromBody] EmployeeCreateRequest request) =>
            {
                var result = await _empRepository.AddEmployee(request);

                return Results.Ok(result);
            });
        }
    }
}

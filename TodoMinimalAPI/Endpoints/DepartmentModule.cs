﻿using FluentValidation;
using TodoMinimalAPI.Models.Requests;
using TodoMinimalAPI.Models;
using TodoMinimalAPI.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace TodoMinimalAPI.Endpoints
{
    public static class DepartmentModule
    {
        public static void AddDepartmentEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/departments", async (IDepartmentRepository _departmentRepository, ILogger<Program> _logger) =>
            {
                var departmentsList = await _departmentRepository.GetDepartments();

                _logger.LogInformation("Retrieved {DepartmentCount} departments successfully", departmentsList.Count());
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

                if (result)
                    return Results.Ok(result);

                return Results.BadRequest(new
                {
                    Success = false,
                    Error = "Department already exists"
                });

            }).Accepts<DepartmentCreateRequest>("application/json").Produces<bool>(200);

            app.MapGet("api/departments/{id:guid}", async (IDepartmentRepository _departmentRepository, Guid id) =>
            {
                var result = await _departmentRepository.GetDepartmentById(id);

                if (result != null)
                    return Results.Ok(result);

                return Results.NotFound($"Department with the requested id {id} does not exist!");

            }).Produces<Department>(200).Produces(404);

            app.MapPut("api/departments", async (IValidator<DepartmentUpdateRequest> _validator, IDepartmentRepository _departmentRepository, DepartmentUpdateRequest request) =>
            {
                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return Results.BadRequest(errorMessages);
                }

                var result = await _departmentRepository.UpdateDepartment(request);

                return Results.Ok(result);

            }).Accepts<DepartmentUpdateRequest>("application/json");

            //comment to check github action
            app.MapDelete("api/departments/{id:guid}", async (IDepartmentRepository _departmentRepository, Guid id) =>
            {
                var result = await _departmentRepository.DeleteDepartment(id);

                return Results.Ok(result);
            });
        }
    }
}

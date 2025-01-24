using Microsoft.EntityFrameworkCore;
using System.Net;
using TodoMinimalAPI.Data;
using TodoMinimalAPI.Models;
using TodoMinimalAPI.Models.Requests;
using TodoMinimalAPI.Models.Responses;
using TodoMinimalAPI.Repository.Interface;

namespace TodoMinimalAPI.Repository.Services
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EmployeeRepository> _logger;
        private readonly APIResponse _apiResponse;

        public EmployeeRepository(AppDbContext context, ILogger<EmployeeRepository> logger, APIResponse apiResponse)
        {
            _context = context;
            _logger = logger;
            _apiResponse = apiResponse;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            try
            {
                var employees = await _context.Employees.ToListAsync();
                return employees;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while retrieving all employees");
                _apiResponse.Success = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                return null;
            }

        }

        public async Task<bool> AddEmployee(EmployeeCreateRequest employee)
        {
            Employee newEmp = new Employee();

            newEmp.Id = Guid.NewGuid();
            newEmp.FirstName = employee.FirstName;
            newEmp.LastName = employee.LastName;
            newEmp.Email = employee.Email;
            newEmp.DateOfBirth = employee.DateOfBirth;

            try
            {
                var department = await _context.Departments.FindAsync(employee.DepartmentId);
                newEmp.DepartmentId = department.Id;
                var result = await _context.Employees.AddAsync(newEmp);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot insert the record");
                _apiResponse.Success = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                return false;
            }
        }

        Task<bool> IEmployeeRepository.DeleteEmployee(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<Employee> IEmployeeRepository.GetEmployee(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<Employee> IEmployeeRepository.UpdateEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}

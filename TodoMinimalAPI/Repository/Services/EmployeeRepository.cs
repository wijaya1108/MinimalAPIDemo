using Microsoft.EntityFrameworkCore;
using TodoMinimalAPI.Data;
using TodoMinimalAPI.Models;
using TodoMinimalAPI.Models.Requests;
using TodoMinimalAPI.Repository.Interface;

namespace TodoMinimalAPI.Repository.Services
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(AppDbContext context, ILogger<EmployeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            return employees;
        }

        public async Task<bool> AddEmployee(EmployeeCreateRequest employee)
        {
            Employee newEmp = new Employee();

            newEmp.Id = Guid.NewGuid();
            newEmp.FirstName = employee.FirstName;
            newEmp.LastName = employee.LastName;
            newEmp.Email = employee.Email;
            newEmp.DateOfBirth = employee.DateOfBirth;

            var department = await _context.Departments.FindAsync(employee.DepartmentId);

            newEmp.DepartmentId = department.Id;

            try
            {
                var result = await _context.Employees.AddAsync(newEmp);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot insert the record");
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

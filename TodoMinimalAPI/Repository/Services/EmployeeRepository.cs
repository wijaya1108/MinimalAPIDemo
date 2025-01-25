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
        private readonly IDepartmentRepository _departmentRepository;

        public EmployeeRepository(AppDbContext context, ILogger<EmployeeRepository> logger, IDepartmentRepository departmentRepository)
        {
            _context = context;
            _logger = logger;
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<EmployeeResponse>> GetEmployees()
        {
            var employees = await _context.Employees
                .Join(
                _context.Departments,
                e => e.DepartmentId,
                d => d.Id,
                (e, d) => new EmployeeResponse
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    DateOfBirth = e.DateOfBirth,
                    Email = e.Email,
                    Department = new Department
                    {
                        Id = d.Id,
                        DepartmentName = d.DepartmentName
                    }
                }).ToListAsync();

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

            var result = await _context.Employees.AddAsync(newEmp);
            await _context.SaveChangesAsync();

            return true;
        }

        Task<bool> IEmployeeRepository.DeleteEmployee(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Employee> GetEmployee(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee != null)
            {
                return employee;
            }
            else
                return null;
        }

        Task<Employee> IEmployeeRepository.UpdateEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}

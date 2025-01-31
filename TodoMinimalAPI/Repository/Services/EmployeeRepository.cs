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

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeResponse>> GetEmployees()
        {
            var employees = await _context.Employees
                .AsNoTracking()
                .Join(
                _context.Departments.AsNoTracking(),
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

            //using Eager Loading
            //var employees = await _context.Employees.Include(e => e.Department)
            //    .Select(e => new EmployeeResponse
            //    {
            //        Id = e.Id,
            //        FirstName = e.FirstName,
            //        LastName = e.LastName,
            //        Email = e.Email,
            //        DateOfBirth = e.DateOfBirth,
            //        Department = e.Department
            //    }).ToListAsync();

            //using lazy loading
            //var employees = await _context.Employees
            //    .Select(e => new EmployeeResponse
            //    {
            //        Id = e.Id,
            //        FirstName = e.FirstName,
            //        LastName = e.LastName,
            //        DateOfBirth = e.DateOfBirth,
            //        Email = e.Email,
            //        DepartmentId = e.DepartmentId
            //    }).ToListAsync();

            //foreach (var employee in employees)
            //{
            //    employee.Department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == employee.DepartmentId);
            //}

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

        public async Task<EmployeeResponse> GetEmployee(Guid id)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Join(_context.Departments.AsNoTracking(),
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
                }).FirstOrDefaultAsync();

            //var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

            //await _context.Entry(employee).Reference(e => e.Department).LoadAsync();

            return employee;
        }

        Task<Employee> IEmployeeRepository.UpdateEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}

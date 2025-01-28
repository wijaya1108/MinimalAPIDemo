using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoMinimalAPI.Data;
using TodoMinimalAPI.Models;
using TodoMinimalAPI.Models.Requests;
using TodoMinimalAPI.Repository.Interface;

namespace TodoMinimalAPI.Repository.Services
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<DepartmentRepository> _logger;

        public DepartmentRepository(AppDbContext dbContext, ILogger<DepartmentRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> AddDepartment(DepartmentCreateRequest department)
        {
            var existingDepartments = await GetDepartments();

            var existingDepartment = existingDepartments.FirstOrDefault(d => d.DepartmentName == department.DepartmentName);

            if (existingDepartment != null)
            {
                return false;
            }

            Department newDepartment = new();
            newDepartment.Id = Guid.NewGuid();
            newDepartment.DepartmentName = department.DepartmentName;

            var result = await _dbContext.Departments.AddAsync(newDepartment);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDepartment(Guid id)
        {
            var department = await _dbContext.Departments.FirstOrDefaultAsync(d => d.Id == id);

            if (department != null)
            {
                _dbContext.Departments.Remove(department);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<Department?> GetDepartmentById(Guid id)
        {
            var department = await _dbContext.Departments.FindAsync(id);

            return department;
        }

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            var departments = await _dbContext.Departments.AsNoTracking().ToListAsync();

            return departments;
        }

        public async Task<Department> UpdateDepartment(DepartmentUpdateRequest department)
        {

            var existingDepartment = await _dbContext.Departments.FirstOrDefaultAsync(d => d.Id == department.Id);

            if (existingDepartment != null)
            {
                var newDepartment = new Department
                {
                    Id = department.Id,
                    DepartmentName = department.DepartmentName
                };
                existingDepartment.DepartmentName = newDepartment.DepartmentName;
                await _dbContext.SaveChangesAsync();
                return existingDepartment;
            }

            return null;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using TodoMinimalAPI.Data;
using TodoMinimalAPI.Models;
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

        public async Task<bool> AddDepartment(Department department)
        {
            try
            {
                var result = await _dbContext.Departments.AddAsync(department);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot insert the department");
                return false;
            }
        }

        public Task<bool> DeleteDepartment(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Department> GetDepartmentById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            var departments = await _dbContext.Departments.ToListAsync();
            _logger.LogInformation("department retrievel successfull");
            return departments;
        }

        public Task<Department> UpdateDepartment(Department department)
        {
            throw new NotImplementedException();
        }
    }
}

using Microsoft.EntityFrameworkCore;
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
            Department newDepartment = new();
            newDepartment.Id = Guid.NewGuid();
            newDepartment.DepartmentName = department.DepartmentName;

            try
            {
                var result = await _dbContext.Departments.AddAsync(newDepartment);
                await _dbContext.SaveChangesAsync();
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

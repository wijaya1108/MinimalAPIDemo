using TodoMinimalAPI.Models;

namespace TodoMinimalAPI.Repository.Interface
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetDepartments();
        Task<Department> GetDepartmentById(Guid id);
        Task<bool> AddDepartment(Department department);
        Task<Department> UpdateDepartment(Department department);
        Task<bool> DeleteDepartment(Guid id);
    }
}

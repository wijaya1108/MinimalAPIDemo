using TodoMinimalAPI.Models;
using TodoMinimalAPI.Models.Requests;

namespace TodoMinimalAPI.Repository.Interface
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetDepartments();
        Task<Department> GetDepartmentById(Guid id);
        Task<bool> AddDepartment(DepartmentCreateRequest department);
        Task<Department> UpdateDepartment(DepartmentUpdateRequest department);
        Task<bool> DeleteDepartment(Guid id);
    }
}

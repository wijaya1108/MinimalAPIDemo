using TodoMinimalAPI.Models;
using TodoMinimalAPI.Models.Requests;
using TodoMinimalAPI.Models.Responses;

namespace TodoMinimalAPI.Repository.Interface
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeResponse>> GetEmployees();
        Task<Employee> GetEmployee(Guid id);
        Task<bool> AddEmployee(EmployeeCreateRequest employee);
        Task<Employee> UpdateEmployee(Employee employee);
        Task<bool> DeleteEmployee(Guid id);
    }
}

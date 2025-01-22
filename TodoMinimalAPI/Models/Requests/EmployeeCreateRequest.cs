namespace TodoMinimalAPI.Models.Requests
{
    public class EmployeeCreateRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid DepartmentId { get; set; }
    }
}

namespace TodoMinimalAPI.Models.Responses
{
    public class EmployeeResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}

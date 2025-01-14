namespace TodoMinimalAPI.Models.Requests
{
    public class DepartmentUpdateRequest
    {
        public Guid Id { get; set; }
        public string DepartmentName { get; set; }
    }
}

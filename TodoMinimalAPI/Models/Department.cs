using System.Diagnostics;

namespace TodoMinimalAPI.Models
{
    [DebuggerDisplay("{DepartmentName}")]
    public class Department
    {
        public Guid Id { get; set; }
        public string DepartmentName { get; set; }
    }
}

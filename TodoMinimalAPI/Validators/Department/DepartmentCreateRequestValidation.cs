using FluentValidation;
using TodoMinimalAPI.Models.Requests;

namespace TodoMinimalAPI.Validators.Department
{
    public class DepartmentCreateRequestValidation : AbstractValidator<DepartmentCreateRequest>
    {
        public DepartmentCreateRequestValidation()
        {
            RuleFor(x => x.DepartmentName).NotEmpty().WithMessage("Department name is required");
        }
    }
}

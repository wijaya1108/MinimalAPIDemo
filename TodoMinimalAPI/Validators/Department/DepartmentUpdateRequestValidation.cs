using FluentValidation;
using TodoMinimalAPI.Models.Requests;

namespace TodoMinimalAPI.Validators.Department
{
    public class DepartmentUpdateRequestValidation : AbstractValidator<DepartmentUpdateRequest>
    {
        public DepartmentUpdateRequestValidation()
        {
            RuleFor(d => d.Id).NotEmpty().WithMessage("Department Id is required");
            RuleFor(d => d.DepartmentName).NotEmpty().WithMessage("Department name is required");
        }
    }
}

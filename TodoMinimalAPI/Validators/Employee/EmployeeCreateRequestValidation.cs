using FluentValidation;
using TodoMinimalAPI.Models.Requests;

namespace TodoMinimalAPI.Validators.Employee
{
    public class EmployeeCreateRequestValidation : AbstractValidator<EmployeeCreateRequest>
    {
        public EmployeeCreateRequestValidation()
        {
            RuleFor(e => e.FirstName).NotEmpty().WithMessage("First name is required");
            RuleFor(e => e.LastName).NotEmpty().WithMessage("Last name is required");
        }
    }
}

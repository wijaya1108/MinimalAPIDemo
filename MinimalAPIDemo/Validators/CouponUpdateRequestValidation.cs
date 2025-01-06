using FluentValidation;
using MinimalAPIDemo.Models.DTO;

namespace MinimalAPIDemo.Validators
{
    public class CouponUpdateRequestValidation : AbstractValidator<CouponUpdateRequest>
    {
        public CouponUpdateRequestValidation()
        {
            RuleFor(model => model.Id).NotEmpty().WithMessage("Id is required");
            RuleFor(model => model.Name).NotEmpty().WithMessage("Name is required");
        }
    }
}

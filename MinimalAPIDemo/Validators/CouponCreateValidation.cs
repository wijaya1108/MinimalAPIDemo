using FluentValidation;
using MinimalAPIDemo.Models.DTO;

namespace MinimalAPIDemo.Validators
{
    public class CouponCreateValidation : AbstractValidator<CouponCreateDTO>
    {
        public CouponCreateValidation()
        {
            RuleFor(model => model.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(model => model.Percent).InclusiveBetween(1, 100);
        }
    }
}

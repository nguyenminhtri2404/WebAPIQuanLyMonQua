using FluentValidation;
using GiftManagement_Version2.Models;

namespace GiftManagement_Version2.Validators
{
    public class GiftValidator : AbstractValidator<GiftRequest>
    {
        public GiftValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.GiftType).NotEmpty().WithMessage("GiftType is required");
            RuleFor(x => x.Quantity).NotEmpty().WithMessage("Quantity is required");
            RuleFor(x => x.Point)
                .NotEmpty()
                .WithMessage("Point is required")
                .GreaterThan(0).WithMessage("Point must be greater than 0");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required")
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Start date must be today or in the future")
                .LessThan(x => x.EndDate).WithMessage("Start date must be before end date");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required")
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("End date must be today or in the future")
                .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");
        }
    }
}

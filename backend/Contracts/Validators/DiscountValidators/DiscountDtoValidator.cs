using Contracts.DTOs.DiscountDTOs;
using FluentValidation;

public class DiscountDtoValidator : AbstractValidator<DiscountDto>
{
    public DiscountDtoValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Discount code is required.")
            .MaximumLength(50).WithMessage("Discount code must not exceed 50 characters.");

        RuleFor(x => x.Percentage)
            .GreaterThan(0).WithMessage("Percentage must be greater than 0.")
            .LessThanOrEqualTo(1).WithMessage("Percentage must be 1.0 or less (e.g., 0.2 for 20%).");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}

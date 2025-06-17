using Contracts.DTOs.DiscountDTOs;
using FluentValidation;

public class UserDiscountDtoValidator : AbstractValidator<UserDiscountDto>
{
    public UserDiscountDtoValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("UserId must be greater than 0.");

        RuleFor(x => x.DiscountId)
            .GreaterThan(0).WithMessage("DiscountId must be greater than 0.");

        RuleFor(x => x.ExpirationDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Expiration date must be in the future.");
    }
}

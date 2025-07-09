using Contracts.DTOs.HallDTOs;
using FluentValidation;

public class UpdateHallDtoValidator : AbstractValidator<UpdateHallDto>
{
    public UpdateHallDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Hall name is required.")
            .MaximumLength(100)
            .WithMessage("Hall name must not exceed 100 characters.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .WithMessage("Capacity must be greater than 0.");

        RuleFor(x => x.SeatsCount)
            .GreaterThan(0)
            .WithMessage("SeatsCount must be greater than 0.")
            .LessThanOrEqualTo(x => x.Capacity)
            .WithMessage("SeatsCount cannot be greater than Capacity.");
    }
}

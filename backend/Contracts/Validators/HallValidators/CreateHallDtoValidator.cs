using Contracts.DTOs.HallDTOs;
using FluentValidation;

public class CreateHallDtoValidator : AbstractValidator<CreateHallDto>
{
    public CreateHallDtoValidator()
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
            .WithMessage("SeatsCount must be greater than 0.");

        RuleFor(x => x)
            .Must(x => x.SeatsCount <= x.Capacity)
            .WithMessage("SeatsCount cannot exceed Capacity.");
    }
}

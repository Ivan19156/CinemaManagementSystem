using Contracts.DTOs.HallDTOs;
using FluentValidation;

public class HallDtoValidator : AbstractValidator<HallDto>
{
    public HallDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Hall name is required.")
            .MaximumLength(100)
            .WithMessage("Hall name must not exceed 100 characters.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .WithMessage("Capacity must be greater than 0.");

        RuleFor(x => x.SeatsCount)
            .NotNull()
            .WithMessage("SeatsCount must be provided.")
            .Must(BeANumber)
            .WithMessage("SeatsCount must be a number.")
            .Must((dto, value) => Convert.ToInt32(value) <= dto.Capacity)
            .WithMessage("SeatsCount cannot exceed Capacity.");
    }

    private bool BeANumber(object value)
    {
        return int.TryParse(value?.ToString(), out _);
    }
}

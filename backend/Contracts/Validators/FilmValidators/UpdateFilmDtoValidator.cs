using Contracts.DTOs.FilmDTOs;
using FluentValidation;

public class UpdateFilmDtoValidator : AbstractValidator<UpdateFilmDto>
{
    public UpdateFilmDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Film ID must be greater than 0.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(150).WithMessage("Title must not exceed 150 characters.");

        RuleFor(x => x.Genre)
            .NotEmpty().WithMessage("Genre is required.")
            .MaximumLength(100).WithMessage("Genre must not exceed 100 characters.");

        RuleFor(x => x.Director)
            .NotEmpty().WithMessage("Director is required.")
            .MaximumLength(100).WithMessage("Director must not exceed 100 characters.");

        RuleFor(x => x.ReleaseDate)
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Release date cannot be in the future.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");
    }
}

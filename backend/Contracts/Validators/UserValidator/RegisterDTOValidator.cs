using Contracts.DTOs.UsersDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Validators.UserValidator;

public class RegisterDTOValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is reguired.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}

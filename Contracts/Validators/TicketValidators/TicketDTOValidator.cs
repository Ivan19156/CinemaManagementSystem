using FluentValidation;
using FluentValidation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Contracts.DTOs.TicketDTOs;

namespace Contracts.Validators.TicketValidators;
using FluentValidation;

public class TicketDtoValidator : AbstractValidator<TicketDto>
{
    public TicketDtoValidator()
    {
        RuleFor(t => t.SessionId)
            .GreaterThan(0)
            .WithMessage("SessionId must be greater than 0.");

        RuleFor(t => t.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0.");

        RuleFor(t => t.SeatNumber)
            .NotEmpty()
            .WithMessage("SeatNumber is required.")
            .MaximumLength(100)
            .WithMessage("SeatNumber cannot exceed 10 characters.");

        RuleFor(t => t.PurchaseDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("PurchaseDate cannot be in the future.");

        RuleFor(t => t.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be a non-negative number.");
    }
}


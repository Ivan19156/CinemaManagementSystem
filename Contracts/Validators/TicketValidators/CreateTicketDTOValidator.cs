using Contracts.DTOs.TicketDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Validators.TicketValidators;

public class CreateTicketDTOValidator : AbstractValidator<CreateTicketDto>
{
    public CreateTicketDTOValidator()
    {
        RuleFor(x => x.SessionId)
            .GreaterThan(0).WithMessage("SessionId must be greater than 0.");
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("UserId must be greater than 0.");
        RuleFor(x => x.SeatNumber)
            .NotEmpty().WithMessage("SeatNumber is required.");
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

    }
}

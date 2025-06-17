using Contracts.DTOs.SaleDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Validators.SaleValidators;

public class CreateSaleDtoValidator : AbstractValidator<CreateSaleDto>
{
    public CreateSaleDtoValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0.");

        RuleFor(x => x.TicketId)
            .GreaterThan(0)
            .WithMessage("TicketId must be greater than 0.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0.");

        RuleFor(x => x.TicketsCount)
            .GreaterThan(0)
            .WithMessage("TicketsCount must be greater than 0.");

        RuleFor(x => x.TotalPrice)
            .GreaterThan(0)
            .WithMessage("TotalPrice must be greater than 0.");

        RuleFor(x => x.SaleDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("SaleDate cannot be in the future.");
    }
}

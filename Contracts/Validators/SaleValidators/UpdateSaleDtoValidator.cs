using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.DTOs.SaleDTOs;
using FluentValidation;

public class UpdateSaleDtoValidator : AbstractValidator<UpdateSaleDto>
{
    public UpdateSaleDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Sale ID must be greater than 0.");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0.");

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

using Contracts.DTOs.SaleDTOs;
using FluentValidation;
namespace Contracts.Validators.SaleValidators;
public class SaleDtoValidator : AbstractValidator<SaleDto>
{
    public SaleDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Sale ID must be greater than 0.");

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

        RuleFor(x => x.TicketIds)
            .NotNull()
            .WithMessage("TicketIds must not be null.")
            .Must(list => list.Count > 0)
            .WithMessage("TicketIds must contain at least one item.")
            .Must(list => list.All(id => id > 0))
            .WithMessage("All TicketIds must be greater than 0.");
    }
}

using Contracts.DTOs.SessionDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Validators.SessionValidators
{
    public class SessionDtoValidator : AbstractValidator<CreateSessionDto>
    {

        public SessionDtoValidator()
        {
            RuleFor(x => x.FilmId)
                .GreaterThan(0).WithMessage("FilmId must be greater than 0.");
            RuleFor(x => x.StartTime)
                .GreaterThan(DateTime.Now).WithMessage("StartTime must be in the future.");
            RuleFor(x => x.HallId)
                .GreaterThan(0).WithMessage("HallId must be greater than 0.");
            RuleFor(x => x.TicketPrice)
                .GreaterThan(0).WithMessage("TicketPrice must be greater than 0.");
        }
    }
}

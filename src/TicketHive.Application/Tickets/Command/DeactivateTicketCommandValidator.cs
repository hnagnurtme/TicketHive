using FluentValidation;

namespace TicketHive.Application.Tickets;

public class DeactivateTicketCommandValidator : AbstractValidator<DeactivateTicketCommand>
{
    public DeactivateTicketCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty()
            .WithMessage("Ticket ID is required");

        RuleFor(x => x.DeactivatedBy)
            .NotEmpty()
            .WithMessage("DeactivatedBy is required");
    }
}
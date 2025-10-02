using FluentValidation;

namespace TicketHive.Application.Tickets;

public class GetTicketsByEventIdQueryValidator : AbstractValidator<GetTicketsByEventIdQuery>
{
    public GetTicketsByEventIdQueryValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event ID is required");
    }
}
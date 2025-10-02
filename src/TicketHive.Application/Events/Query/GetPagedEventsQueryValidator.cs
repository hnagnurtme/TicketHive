using FluentValidation;

namespace TicketHive.Application.Events;

public class GetPagedEventsQueryValidator : AbstractValidator<GetPagedEventsQuery>
{
    public GetPagedEventsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must be between 1 and 100");

        RuleFor(x => x.StartDateFrom)
            .LessThan(x => x.StartDateTo)
            .When(x => x.StartDateFrom.HasValue && x.StartDateTo.HasValue)
            .WithMessage("Start date from must be before start date to");
    }
}
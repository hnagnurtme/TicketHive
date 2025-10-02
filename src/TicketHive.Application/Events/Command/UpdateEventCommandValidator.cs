using FluentValidation;

namespace TicketHive.Application.Events;

public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("Event ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Event name is required")
            .MaximumLength(200)
            .WithMessage("Event name cannot exceed 200 characters");

        RuleFor(x => x.Location)
            .NotEmpty()
            .WithMessage("Location is required")
            .MaximumLength(500)
            .WithMessage("Location cannot exceed 500 characters");

        RuleFor(x => x.StartTime)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Start time must be in the future");

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be after start time");

        RuleFor(x => x.VenueCapacity)
            .GreaterThan(0)
            .When(x => x.VenueCapacity.HasValue)
            .WithMessage("Venue capacity must be greater than 0");

        RuleFor(x => x.SaleEndTime)
            .GreaterThan(x => x.SaleStartTime)
            .When(x => x.SaleStartTime.HasValue && x.SaleEndTime.HasValue)
            .WithMessage("Sale end time must be after sale start time");

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage("Description cannot exceed 2000 characters");
    }
}
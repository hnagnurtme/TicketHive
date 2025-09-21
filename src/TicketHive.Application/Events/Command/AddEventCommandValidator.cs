namespace TicketHive.Application.Events;
using FluentValidation;
using TicketHive.Application.Events.Command;

public class AddEventCommandValidator : AbstractValidator<AddEventCommand>
{
    public AddEventCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Event name is required.")
            .MaximumLength(100).WithMessage("Event name must not exceed 100 characters.");
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Event slug is required.")
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug must be URL friendly (lowercase letters, numbers, hyphens).")
            .MaximumLength(100).WithMessage("Event slug must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(200).WithMessage("Location must not exceed 200 characters.");

        RuleFor(x => x.StartTime)
            .LessThan(x => x.EndTime).WithMessage("Start time must be before end time.");

        RuleFor(x => x.VenueCapacity)
            .GreaterThan(0).When(x => x.VenueCapacity.HasValue).WithMessage("Venue capacity must be greater than 0.");

        RuleFor(x => x.SaleStartTime)
            .LessThan(x => x.SaleEndTime).When(x => x.SaleStartTime.HasValue && x.SaleEndTime.HasValue)
            .WithMessage("Sale start time must be before sale end time.");

        RuleFor(x => x.ImageUrl)
            .Must(uri => uri == null || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("Image URL must be a valid URL.");
    }
}
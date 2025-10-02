using FluentValidation;

namespace TicketHive.Application.Tickets;

public class UpdateTicketCommandValidator : AbstractValidator<UpdateTicketCommand>
{
    public UpdateTicketCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty()
            .WithMessage("Ticket ID is required");

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Ticket type is required")
            .MaximumLength(50)
            .WithMessage("Ticket type cannot exceed 50 characters");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Ticket name is required")
            .MaximumLength(200)
            .WithMessage("Ticket name cannot exceed 200 characters");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price cannot be negative");

        RuleFor(x => x.TotalQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Total quantity cannot be negative");

        RuleFor(x => x.MinPurchase)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Minimum purchase must be at least 1");

        RuleFor(x => x.MaxPurchase)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Maximum purchase must be at least 1")
            .GreaterThanOrEqualTo(x => x.MinPurchase)
            .WithMessage("Maximum purchase cannot be less than minimum purchase");

        RuleFor(x => x.OriginalPrice)
            .GreaterThanOrEqualTo(x => x.Price)
            .When(x => x.OriginalPrice.HasValue)
            .WithMessage("Original price cannot be less than current price");

        RuleFor(x => x.SaleEndTime)
            .GreaterThan(x => x.SaleStartTime)
            .When(x => x.SaleStartTime.HasValue && x.SaleEndTime.HasValue)
            .WithMessage("Sale end time must be after sale start time");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.UpdatedBy)
            .NotEmpty()
            .WithMessage("UpdatedBy is required");
    }
}
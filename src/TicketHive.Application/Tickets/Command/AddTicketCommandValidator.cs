namespace TicketHive.Application.Ticket;

using FluentValidation;
using TicketHive.Application.Tickets;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("EventId is required");

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Type is required")
            .MaximumLength(100)
            .WithMessage("Type must not exceed 100 characters");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be greater than or equal to 0");

        RuleFor(x => x.TotalQuantity)
            .GreaterThan(0)
            .WithMessage("TotalQuantity must be greater than 0");

        RuleFor(x => x.MinPurchase)
            .GreaterThan(0)
            .WithMessage("MinPurchase must be greater than 0")
            .LessThanOrEqualTo(x => x.MaxPurchase)
            .WithMessage("MinPurchase must be less than or equal to MaxPurchase");

        RuleFor(x => x.MaxPurchase)
            .GreaterThan(0)
            .WithMessage("MaxPurchase must be greater than 0")
            .GreaterThanOrEqualTo(x => x.MinPurchase)
            .WithMessage("MaxPurchase must be greater than or equal to MinPurchase");

        RuleFor(x => x.OriginalPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.OriginalPrice.HasValue)
            .WithMessage("OriginalPrice must be greater than or equal to 0");

        RuleFor(x => x.SaleEndTime)
            .GreaterThan(x => x.SaleStartTime!.Value)
            .When(x => x.SaleStartTime.HasValue && x.SaleEndTime.HasValue)
            .WithMessage("SaleEndTime must be later than SaleStartTime");
    }
}

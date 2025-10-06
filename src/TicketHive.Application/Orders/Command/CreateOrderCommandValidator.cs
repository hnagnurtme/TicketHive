namespace TicketHive.Application.Orders;
using FluentValidation;
using TicketHive.Application.Events.Command;
using TicketHive.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
       RuleFor(x => x.UserId)
           .NotEmpty().WithMessage("UserId is required.")
           .NotEqual(Guid.Empty).WithMessage("UserId cannot be an empty GUID.");

       RuleFor(x => x.Items)
           .NotEmpty().WithMessage("At least one order item is required.")
           .Must(items => items != null && items.Count > 0).WithMessage("Items list cannot be empty.");

       RuleForEach(x => x.Items).SetValidator(new CreateOrderItemValidator());

       RuleFor(x => x.PaymentProvider)
           .NotEmpty().WithMessage("PaymentProvider is required.")
           .MaximumLength(100).WithMessage("PaymentProvider cannot exceed 100 characters.");

       RuleFor(x => x.CouponCode)
           .MaximumLength(50).WithMessage("CouponCode cannot exceed 50 characters.")
           .When(x => !string.IsNullOrEmpty(x.CouponCode));
    }
}
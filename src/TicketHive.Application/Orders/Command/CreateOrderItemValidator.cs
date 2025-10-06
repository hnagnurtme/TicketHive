namespace TicketHive.Application.Orders;
using FluentValidation;
using TicketHive.Application.Events.Command;
using TicketHive.Application.Orders.Commands.CreateOrder;

public class CreateOrderItemValidator : AbstractValidator<CreateOrderItem>
{
    public CreateOrderItemValidator()
    {
            RuleFor(x => x.TicketId)
                .NotEmpty().WithMessage("TicketId is required.")
                .NotEqual(Guid.Empty).WithMessage("TicketId cannot be an empty GUID.");
    
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}
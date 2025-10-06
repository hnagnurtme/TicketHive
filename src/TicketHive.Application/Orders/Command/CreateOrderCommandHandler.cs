using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Common.Interfaces;
using TicketHive.Application.Common.Interfaces.Payment;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Application.Orders.Commands.CreateOrder;
using TicketHive.Application.Orders.Result;
using TicketHive.Domain.Entities;
using TicketHive.Domain.Exceptions.Base;
using TicketHive.Application.Exceptions; 
using Error = ErrorOr.Error;
using TicketHive.Application.Common.Exceptions;

namespace TicketHive.Application.Orders.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ErrorOr<OrderResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateOrderCommandHandler> _logger;
    private readonly IPaymentService _paymentService;

    public CreateOrderCommandHandler(
        IUnitOfWork unitOfWork, 
        ILogger<CreateOrderCommandHandler> logger,
        IPaymentService paymentService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _paymentService = paymentService;
    }

    public async Task<ErrorOr<OrderResult>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var totalAmount = 0m;
        var orderItems = new List<OrderItem>();
        
        // Bắt đầu Transaction
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var tickets = new Dictionary<Guid, TicketHive.Domain.Entities.Ticket>();
            
            foreach (var item in request.Items)
            {
                // Giả định GetByIdWithInventoryAsync tải Ticket và Inventory
                var ticket = await _unitOfWork.Tickets.GetByIdWithInventoryAsync(item.TicketId);
                
                // === Kiểm tra nghiệp vụ: Sử dụng THROW (Chỉ ném lỗi đầu tiên) ===
                if (ticket is null)
                {
                    throw new NotFoundException($"Ticket with ID {item.TicketId} not found."); 
                }
                
                if (!ticket.IsOnSale(DateTime.UtcNow))
                {
                    throw new TicketNotOnSaleException($"{ticket.Name} is not currently on sale.");
                }
                if (ticket.Inventory.RemainingQuantity < item.Quantity)
                {
                    throw new InsufficientInventoryException($"Insufficient stock for {ticket.Name}. Only {ticket.Inventory.RemainingQuantity} available.");
                }
                if (item.Quantity < ticket.MinPurchase || item.Quantity > ticket.MaxPurchase)
                {
                    throw new DomainException($"Quantity must be between {ticket.MinPurchase} and {ticket.MaxPurchase}.", "QuantityLimit");
                }
                
                var unitPrice = ticket.Price;
                var subTotal = unitPrice * item.Quantity;
                totalAmount += subTotal;

                orderItems.Add(new OrderItem(ticket.Id, item.Quantity, unitPrice));
                tickets.Add(ticket.Id, ticket);
            }
            
            // === Tạo Order và Khởi tạo Thanh toán ===
            var order = new Order(request.UserId, totalAmount, request.PaymentProvider, orderItems);

            var paymentResult = await _paymentService.InitiatePaymentAsync(order, request.PaymentProvider);

            // === Lưu vào Database ===
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CommitTransactionAsync(); 
            
            // === Ánh xạ và Trả về Result ===
            var resultItems = order.Items.Select(oi => 
                new OrderItemResult(
                    oi.TicketId, 
                    tickets.GetValueOrDefault(oi.TicketId)?.Name ?? "Unknown Ticket", 
                    oi.Quantity, 
                    oi.UnitPrice, 
                    oi.SubTotal)
            ).ToList();
            
            return new OrderResult(
                order.Id,
                order.Status.ToString(),
                order.TotalAmount,
                order.PaymentProvider!,
                paymentResult.PaymentUrl, 
                order.CreatedAt,
                resultItems
            );
        }
        catch (Exception ex)
        {
            // Bắt buộc phải có khối catch này để ROLLBACK!
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Transaction rolled back due to error during order creation.");
            
            // Ném lại ngoại lệ để nó được bắt bởi tầng bên ngoài (Middleware/Filter/etc.)
            throw; 
        }
    }
}
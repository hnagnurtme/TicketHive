using TicketHive.Domain.Exceptions.Base;

namespace TicketHive.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid TicketId { get; private set; }

    public int Quantity { get; private set; }

    // Quan trọng: Lưu trữ giá vé tại thời điểm mua (Giá lịch sử)
    public decimal UnitPrice { get; private set; }
    public decimal SubTotal { get; private set; } // Quantity * UnitPrice

    // === Thuộc tính Quan hệ ===
    public Order Order { get; private set; } = null!;
    public Ticket Ticket { get; private set; } = null!; // Navigation property

    // Private constructor cho ORM
    private OrderItem() { }

    public OrderItem(Guid ticketId, int quantity, decimal unitPrice)
    {
        if (ticketId == Guid.Empty)
            throw new DomainException("Ticket ID is required.", nameof(ticketId));
        if (quantity <= 0)
            throw new DomainException("Quantity must be positive.", nameof(quantity));
        if (unitPrice < 0)
            throw new DomainException("Unit price cannot be negative.", nameof(unitPrice));

        Id = Guid.NewGuid();
        TicketId = ticketId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        SubTotal = quantity * unitPrice;
    }
}
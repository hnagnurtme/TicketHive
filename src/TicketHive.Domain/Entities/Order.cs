namespace TicketHive.Domain.Entities;
using System;
using System.Collections.Generic;
using TicketHive.Domain.Exceptions.Base;



/// <summary>
/// Represents a customer order in the TicketHive system.
/// </summary>
public class Order
{
    // === Thuộc tính Cốt lõi ===
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    
    public DateTime OrderDate { get; private set; } = DateTime.UtcNow;

    public decimal TotalAmount { get; private set; }
    public string Currency { get; private set; } = "VND"; // Mặc định là VND

    // === Thuộc tính Trạng thái & Thanh toán ===
    public OrderStatus Status { get; private set; }
    public string? PaymentProvider { get; private set; } // Ví dụ: MOMO, VNPAY, STRIPE
    public string? TransactionId { get; private set; }     // ID giao dịch từ cổng thanh toán
    public DateTime? PaymentDate { get; private set; }
    
    // === Thuộc tính Quan hệ ===
    public User User { get; private set; } = null!; // Navigation property
    public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>(); // Chi tiết đơn hàng

    // === Audit Fields ===
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    // Private constructor cho ORM (Entity Framework Core)
    private Order() { } 

    /// <summary>
    /// Tạo một đơn hàng mới ở trạng thái chờ thanh toán.
    /// </summary>
    public Order(Guid userId, decimal totalAmount, string paymentProvider, ICollection<OrderItem> items)
    {
        if (userId == Guid.Empty)
            throw new DomainException("Order must be associated with a valid User.", nameof(userId));
        if (totalAmount <= 0)
            throw new DomainException("Total amount must be greater than zero.", nameof(totalAmount));
        if (items == null || items.Count == 0)
            throw new DomainException("Order must contain at least one item.", nameof(items));

        Id = Guid.NewGuid();
        UserId = userId;
        TotalAmount = totalAmount;
        Status = OrderStatus.PENDING_PAYMENT;
        PaymentProvider = paymentProvider;
        Items = items;
        
        Touch();
    }

    // === Domain Behaviors (Hành vi) ===

    public void MarkAsPaid(string transactionId, DateTime paymentDate)
    {
        if (Status != OrderStatus.PENDING_PAYMENT)
            throw new InvalidOperationException($"Cannot mark order as paid. Current status: {Status}.");

        TransactionId = transactionId;
        PaymentDate = paymentDate;
        Status = OrderStatus.PAID;
        Touch();
    }

    public void Cancel()
    {
        if (Status == OrderStatus.PAID || Status == OrderStatus.CONFIRMED)
            throw new InvalidOperationException("Cannot cancel an already paid or confirmed order.");

        Status = OrderStatus.CANCELLED;
        Touch();
    }

    private void Touch() => UpdatedAt = DateTime.UtcNow;
}

/// <summary>
/// Trạng thái của đơn hàng.
/// </summary>
public enum OrderStatus
{
    PENDING_PAYMENT,
    PAID,
    CONFIRMED, 
    CANCELLED,
    REFUNDED
}

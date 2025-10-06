using System;
using System.Collections.Generic;

namespace TicketHive.Application.Orders.Result;

/// <summary>
/// DTO phản hồi sau khi một Order được tạo thành công (Record).
/// </summary>
public record OrderResult(
    /// <summary>
    /// Mã định danh duy nhất của đơn hàng đã được tạo.
    /// </summary>
    Guid OrderId,

    /// <summary>
    /// Trạng thái ban đầu của đơn hàng (Thường là PENDING_PAYMENT).
    /// </summary>
    string Status, // Tên trường đã được giữ lại từ lớp gốc

    /// <summary>
    /// Tổng số tiền cần thanh toán.
    /// </summary>
    decimal TotalAmount,

    /// <summary>
    /// Cổng thanh toán được chọn.
    /// </summary>
    string PaymentProvider,

    /// <summary>
    /// URL mà khách hàng cần được chuyển hướng đến để hoàn tất thanh toán.
    /// Đây là trường quan trọng nhất.
    /// </summary>
    string PaymentUrl,

    /// <summary>
    /// Ngày giờ tạo đơn hàng.
    /// </summary>
    DateTime CreatedAt,
    
    /// <summary>
    /// Danh sách các items đã tạo nếu cần xác nhận chi tiết.
    /// </summary>
    List<OrderItemResult>? Items
)
{
    // Cung cấp giá trị mặc định cho Status trong constructor chính
    public OrderResult(
        Guid orderId, 
        decimal totalAmount, 
        string paymentProvider, 
        string paymentUrl, 
        DateTime createdAt, 
        List<OrderItemResult>? items) 
        : this(orderId, "PENDING_PAYMENT", totalAmount, paymentProvider, paymentUrl, createdAt, items) { }
}

/// <summary>
/// DTO cho chi tiết từng mục vé trong phản hồi đơn hàng (Tùy chọn).
/// </summary>
public record OrderItemResult(
    Guid TicketId,
    string TicketName,
    int Quantity,
    decimal UnitPrice,
    decimal SubTotal
);
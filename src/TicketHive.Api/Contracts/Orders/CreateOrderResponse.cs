using System;
using System.Collections.Generic;

namespace TicketHive.Api.Contracts.Orders;

/// <summary>
/// DTO phản hồi sau khi một Order được tạo thành công.
/// </summary>
public class CreateOrderResponse
{
    /// <summary>
    /// Mã định danh duy nhất của đơn hàng đã được tạo.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Trạng thái ban đầu của đơn hàng (Thường là PENDING_PAYMENT).
    /// </summary>
    public string Status { get; set; } = "PENDING_PAYMENT";

    /// <summary>
    /// Tổng số tiền cần thanh toán.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Cổng thanh toán được chọn.
    /// </summary>
    public string PaymentProvider { get; set; } = string.Empty;

    /// <summary>
    /// URL mà khách hàng cần được chuyển hướng đến để hoàn tất thanh toán.
    /// Đây là trường quan trọng nhất.
    /// </summary>
    public string PaymentUrl { get; set; } = string.Empty;

    /// <summary>
    /// Ngày giờ tạo đơn hàng.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    // Tùy chọn: Có thể bao gồm danh sách các items đã tạo nếu cần xác nhận chi tiết.
    public List<OrderItemResponse>? Items { get; set; }
}

/// <summary>
/// DTO cho chi tiết từng mục vé trong phản hồi đơn hàng (Tùy chọn).
/// </summary>
public class OrderItemResponse
{
    public Guid TicketId { get; set; }
    public string TicketName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal { get; set; }
}
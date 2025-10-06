namespace TicketHive.Api.Contracts.Orders;

using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


/// <summary>
/// DTO (Data Transfer Object) cho yêu cầu tạo đơn hàng mới.
/// </summary>
public class CreateOrderRequest
{
    /// <summary>
    /// Danh sách chi tiết các loại vé và số lượng muốn mua.
    /// </summary>
    [Required]
    [MinLength(1, ErrorMessage = "A request must contain at least one item.")]
    public List<OrderItemRequest> Items { get; set; } = new List<OrderItemRequest>();

    /// <summary>
    /// Phương thức thanh toán mà khách hàng lựa chọn (Ví dụ: MOMO, VNPAY, CreditCard).
    /// </summary>
    [Required]
    [StringLength(50, ErrorMessage = "Payment Provider name cannot exceed 50 characters.")]
    public string PaymentProvider { get; set; } = string.Empty;

    /// <summary>
    /// Mã giảm giá (Tùy chọn).
    /// </summary>
    public string? CouponCode { get; set; } 
}

// ----------------------------------------------------------------------

/// <summary>
/// DTO cho từng mục vé trong yêu cầu tạo đơn hàng.
/// </summary>
public class OrderItemRequest
{
    /// <summary>
    /// Mã định danh duy nhất của loại vé (Ticket.Id).
    /// </summary>
    [Required]
    public Guid TicketId { get; set; }

    /// <summary>
    /// Số lượng vé của loại này muốn mua.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }
}
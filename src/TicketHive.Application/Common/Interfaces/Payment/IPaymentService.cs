using ErrorOr; 
using TicketHive.Domain.Entities;

namespace TicketHive.Application.Common.Interfaces.Payment;

/// <summary>
/// Kết quả trả về sau khi khởi tạo giao dịch thanh toán thành công.
/// </summary>
public record PaymentInitiationResult(
    string PaymentUrl, 
    string? GatewayTransactionId
);

/// <summary>
/// Kết quả trả về sau khi xác minh Webhook (xác nhận thanh toán).
/// </summary>
public record PaymentVerificationResult(
    Guid OrderId,
    decimal Amount,
    string GatewayTransactionId,
    bool IsSuccess
);

/// <summary>
/// Hợp đồng cho dịch vụ giao tiếp với các cổng thanh toán bên ngoài.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Khởi tạo một giao dịch thanh toán, trả về URL để chuyển hướng khách hàng.
    /// </summary>
    /// <param name="order">Thực thể Order cần thanh toán.</param>
    /// <param name="provider">Tên cổng thanh toán (MOMO, VNPAY...).</param>
    /// <returns>PaymentUrl và Transaction ID.</returns>
    Task<PaymentInitiationResult> InitiatePaymentAsync(Order order, string provider);
    
    /// <summary>
    /// Xác minh phản hồi Webhook/Callback từ cổng thanh toán.
    /// </summary>
    /// <param name="provider">Cổng thanh toán đã gửi webhook.</param>
    /// <param name="requestData">Dữ liệu thô nhận được từ webhook (query string, body...).</param>
    /// <returns>Thông tin xác minh giao dịch.</returns>
    Task<ErrorOr<PaymentVerificationResult>> HandleWebhookAsync(string provider, object requestData);
}
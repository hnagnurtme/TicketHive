using ErrorOr;
using TicketHive.Application.Common.Interfaces.Payment;
using TicketHive.Domain.Entities;

namespace TicketHive.Infrastructure.Payment.Adapters;

public class VnPayAdapter : IPaymentAdapter
{
    public string ProviderName => "VNPAY";

    public async Task<PaymentInitiationResult> InitiatePaymentAsync(Order order)
    {
        // === Logic gọi API của VNPAY ===
        // 1. Tạo request URL (thêm các tham số như vnp_TmnCode, vnp_Amount, vnp_Command)
        // 2. Tính toán HashData và thêm vnp_SecureHash.
        
        var gatewayTxnId = $"VNPAY_{DateTime.Now.Ticks}";

        // URL thanh toán giả lập
        var paymentUrl = $"https://mock-vnpay-gateway.com/pay?vnp_TxnRef={order.Id}&vnp_Amount={order.TotalAmount}";
        
        await Task.Delay(50);
        
        return new PaymentInitiationResult(
            PaymentUrl: paymentUrl,
            GatewayTransactionId: gatewayTxnId
        );
    }

    public ErrorOr<PaymentVerificationResult> VerifyWebhook(object requestData)
    {
        // 1. Phân tích requestData (Chủ yếu là QueryString)
        // 2. Xác thực VNPAY HASH CODE (Quan trọng)
        // Nếu hash không khớp, trả về lỗi.

        // Dữ liệu giả định:
        Guid orderId = Guid.Parse("YOUR_PARSED_ORDER_ID"); 
        decimal amount = 1250.50m;
        bool success = true;
        
        if (!success)
        {
            return Error.Failure("VNPAY", "VNPAY transaction failed or was rejected.");
        }

        // 3. Trả về kết quả xác minh thành công
        return new PaymentVerificationResult(
            OrderId: orderId,
            Amount: amount,
            GatewayTransactionId: "VNPAY_TXN_VALID",
            IsSuccess: true
        );
    }
}
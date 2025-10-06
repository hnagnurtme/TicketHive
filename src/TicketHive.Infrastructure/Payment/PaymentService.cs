using TicketHive.Application.Common.Interfaces.Payment;
using TicketHive.Domain.Entities;
using ErrorOr;


namespace TicketHive.Infrastructure.Payment;


public interface IPaymentAdapter
{
    string ProviderName { get; }
    Task<PaymentInitiationResult> InitiatePaymentAsync(Order order);
    ErrorOr<PaymentVerificationResult> VerifyWebhook(object requestData);
}

public class PaymentService : IPaymentService
{
    private readonly IDictionary<string, IPaymentAdapter> _adapters;
    
    public PaymentService(IEnumerable<IPaymentAdapter> adapters)
    {
        _adapters = adapters.ToDictionary(a => a.ProviderName.ToUpperInvariant());
    }

    private IPaymentAdapter GetAdapter(string provider)
    {
        var key = provider.ToUpperInvariant();
        if (!_adapters.TryGetValue(key, out var adapter))
        {
            throw new NotSupportedException($"Payment provider '{provider}' is not supported.");
        }
        return adapter;
    }

    public async Task<PaymentInitiationResult> InitiatePaymentAsync(Order order, string provider)
    {
        var adapter = GetAdapter(provider);
        return await adapter.InitiatePaymentAsync(order);
    }

    public Task<ErrorOr<PaymentVerificationResult>> HandleWebhookAsync(string provider, object requestData)
    {
        var adapter = GetAdapter(provider);
        
        // Gọi phương thức Verify của Adapter cụ thể (ví dụ: MomoAdapter.VerifyWebhook)
        // Đây là nơi logic xác thực chữ ký của cổng thanh toán xảy ra.
        return Task.FromResult(adapter.VerifyWebhook(requestData));
    }
}
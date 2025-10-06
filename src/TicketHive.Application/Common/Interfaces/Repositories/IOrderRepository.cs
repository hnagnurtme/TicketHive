using TicketHive.Domain.Entities;

namespace TicketHive.Application.Common.Interfaces.Repositories;

public interface IOrderRepository : IGenericRepository<Order>
{
    // 1. Truy vấn theo Người dùng
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);
    
     // 2. Truy vấn cho Webhook (Cần Eager Loading Items)
    Task<Order?> GetOrderForPaymentConfirmationAsync(Guid orderId);

    // 3. Truy vấn các đơn hàng cần xử lý tự động
    Task<IEnumerable<Order>> GetPendingOrdersOlderThanAsync(TimeSpan duration);
    
}

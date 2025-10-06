namespace TicketHive.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using TicketHive.Domain.Entities;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Application.Common;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext dbContext) : base(dbContext) { }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId)
    {
        return await _dbContext.Set<Order>()
            .Where(o => o.UserId == userId)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderForPaymentConfirmationAsync(Guid orderId)
    {
        return await _dbContext.Set<Order>()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<IEnumerable<Order>> GetPendingOrdersOlderThanAsync(TimeSpan timeSpan)
    {
        var cutoffTime = DateTime.UtcNow.Subtract(timeSpan);
        return await _dbContext.Set<Order>()
            .Where(o => o.CreatedAt < cutoffTime && o.Status == OrderStatus.PENDING_PAYMENT)
            .ToListAsync();
    }

}
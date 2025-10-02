using Microsoft.EntityFrameworkCore;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Domain.Entities;

namespace TicketHive.Infrastructure.Persistence.Repositories;

public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
{

    public TicketRepository(AppDbContext dbContext) : base(dbContext) { }

    public async Task<IEnumerable<Ticket>> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tickets
            .Where(t => t.EventId == eventId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Ticket?> GetByIdAndEventIdAsync(Guid ticketId, Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tickets
            .FirstOrDefaultAsync(t => t.Id == ticketId && t.EventId == eventId, cancellationToken);
    }
}
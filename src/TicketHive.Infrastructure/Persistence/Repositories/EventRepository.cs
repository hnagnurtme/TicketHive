namespace TicketHive.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

using TicketHive.Domain.Entities;
using TicketHive.Application.Common.Interfaces.Repositories;
public class EventRepository : GenericRepository<Event>, IEventRepository
{

    public EventRepository(AppDbContext dbContext) : base(dbContext) { }

    public async Task<Event?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Events
            .FirstOrDefaultAsync(e => e.Slug == slug, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Events
            .AnyAsync(e => e.Id == eventId, cancellationToken);
    }
}
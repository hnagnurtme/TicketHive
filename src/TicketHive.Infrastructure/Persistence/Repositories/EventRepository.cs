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
}
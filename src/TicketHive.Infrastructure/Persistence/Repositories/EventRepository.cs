namespace TicketHive.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using TicketHive.Domain.Entities;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Application.Common;

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

    public async Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Events
            .OrderBy(e => e.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedResult<Event>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? status = null,
        bool? isFeatured = null,
        string? searchTerm = null,
        DateTime? startDateFrom = null,
        DateTime? startDateTo = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Events.AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(e => e.Status.ToString() == status);
        }

        if (isFeatured.HasValue)
        {
            query = query.Where(e => e.IsFeatured == isFeatured.Value);
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(e => e.Name.Contains(searchTerm) || 
                                   (e.Description != null && e.Description.Contains(searchTerm)) ||
                                   e.Location.Contains(searchTerm));
        }

        if (startDateFrom.HasValue)
        {
            query = query.Where(e => e.StartTime >= startDateFrom.Value);
        }

        if (startDateTo.HasValue)
        {
            query = query.Where(e => e.StartTime <= startDateTo.Value);
        }

        // Get total count
        var totalItems = await query.CountAsync(cancellationToken);

        // Apply pagination
        var events = await query
            .OrderBy(e => e.StartTime)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Event>
        {
            Items = events,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<bool> HasTicketsAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tickets
            .AnyAsync(t => t.EventId == eventId, cancellationToken);
    }
}
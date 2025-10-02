using Microsoft.EntityFrameworkCore;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Domain.Entities;

namespace TicketHive.Infrastructure.Persistence.Repositories;

public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
{

    public TicketRepository(AppDbContext dbContext) : base(dbContext) { }

    // Override GetByIdAsync to include Inventory
    public new async Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tickets
            .Include(t => t.Inventory)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Ticket>> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tickets
            .Include(t => t.Inventory)
            .Where(t => t.EventId == eventId)
            .OrderBy(t => t.SortOrder)
            .ThenBy(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Ticket?> GetByIdAndEventIdAsync(Guid ticketId, Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tickets
            .Include(t => t.Inventory)
            .FirstOrDefaultAsync(t => t.Id == ticketId && t.EventId == eventId, cancellationToken);
    }

    public Task<bool> HasSalesAsync(Guid ticketId, CancellationToken cancellationToken = default)
    {
        // Check if there are any orders/bookings for this ticket
        // This is a placeholder - you may need to adjust based on your actual domain model
        // For now, we'll return false as a safe default
        return Task.FromResult(false);
        
        // If you have order items or ticket bookings, use something like:
        // return _dbContext.OrderItems
        //     .AnyAsync(oi => oi.TicketId == ticketId, cancellationToken);
    }

    public async Task<Ticket?> GetByIdWithInventoryAsync(Guid ticketId, CancellationToken cancellationToken = default)
    {
        // This method is now redundant with GetByIdAsync, but keeping for interface compliance
        return await GetByIdAsync(ticketId, cancellationToken);
    }

    public async Task<(IEnumerable<Ticket> tickets, int totalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        Guid? eventId = null,
        bool? isActive = null,
        string? sortBy = null,
        string? sortDirection = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Tickets
            .Include(t => t.Inventory)
            .AsQueryable();

        // Apply filters
        if (eventId.HasValue)
        {
            query = query.Where(t => t.EventId == eventId.Value);
        }

        if (isActive.HasValue)
        {
            query = query.Where(t => t.IsActive == isActive.Value);
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(t => 
                t.Name.Contains(searchTerm) || 
                t.Type.Contains(searchTerm) || 
                (t.Description != null && t.Description.Contains(searchTerm)));
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply sorting
        query = ApplySorting(query, sortBy, sortDirection);

        // Apply pagination
        var tickets = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (tickets, totalCount);
    }

    private static IQueryable<Ticket> ApplySorting(IQueryable<Ticket> query, string? sortBy, string? sortDirection)
    {
        var isDescending = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);

        return sortBy?.ToLower() switch
        {
            "name" => isDescending ? query.OrderByDescending(t => t.Name) : query.OrderBy(t => t.Name),
            "type" => isDescending ? query.OrderByDescending(t => t.Type) : query.OrderBy(t => t.Type),
            "price" => isDescending ? query.OrderByDescending(t => t.Price) : query.OrderBy(t => t.Price),
            "sortorder" => isDescending ? query.OrderByDescending(t => t.SortOrder) : query.OrderBy(t => t.SortOrder),
            "isactive" => isDescending ? query.OrderByDescending(t => t.IsActive) : query.OrderBy(t => t.IsActive),
            "createdat" or _ => isDescending ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt)
        };
    }
}
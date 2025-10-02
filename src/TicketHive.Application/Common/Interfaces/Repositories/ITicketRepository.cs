namespace TicketHive.Application.Common.Interfaces.Repositories;

using TicketHive.Domain.Entities;

public interface ITicketRepository : IGenericRepository<Ticket>
{
    Task<IEnumerable<Ticket>> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default);
    Task<Ticket?> GetByIdAndEventIdAsync(Guid ticketId, Guid eventId, CancellationToken cancellationToken = default);
    Task<bool> HasSalesAsync(Guid ticketId, CancellationToken cancellationToken = default);
    Task<Ticket?> GetByIdWithInventoryAsync(Guid ticketId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Ticket> tickets, int totalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        Guid? eventId = null,
        bool? isActive = null,
        string? sortBy = null,
        string? sortDirection = null,
        CancellationToken cancellationToken = default);
}
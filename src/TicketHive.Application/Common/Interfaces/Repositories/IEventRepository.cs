namespace TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Domain.Entities;
using TicketHive.Application.Common;

public interface IEventRepository : IGenericRepository<Event>
{
    Task<Event?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid eventId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<Event>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? status = null,
        bool? isFeatured = null,
        string? searchTerm = null,
        DateTime? startDateFrom = null,
        DateTime? startDateTo = null,
        CancellationToken cancellationToken = default);
    Task<bool> HasTicketsAsync(Guid eventId, CancellationToken cancellationToken = default);
}
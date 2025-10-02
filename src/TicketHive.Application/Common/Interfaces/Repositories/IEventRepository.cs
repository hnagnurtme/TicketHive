namespace TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Domain.Entities;

public interface IEventRepository : IGenericRepository<Event>
{
    Task<Event?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid eventId, CancellationToken cancellationToken = default);
}
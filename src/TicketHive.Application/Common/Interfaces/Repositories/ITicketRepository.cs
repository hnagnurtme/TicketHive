namespace TicketHive.Application.Common.Interfaces.Repositories;

using TicketHive.Domain.Entities;

public interface ITicketRepository : IGenericRepository<Ticket>
{
    Task<IEnumerable<Ticket>> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default);
    Task<Ticket?> GetByIdAndEventIdAsync(Guid ticketId, Guid eventId, CancellationToken cancellationToken = default);
}
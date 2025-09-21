namespace TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Domain.Entities;

public interface IEventRepository : IGenericRepository<Event>
{
    Task<Event?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
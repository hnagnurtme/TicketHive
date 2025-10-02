namespace TicketHive.Application.Common.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    public IUserRepository Users { get; }
    public ITokenRepository Tokens { get; }

    public IEventRepository Events { get; }

    public ITicketRepository Tickets { get; }
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
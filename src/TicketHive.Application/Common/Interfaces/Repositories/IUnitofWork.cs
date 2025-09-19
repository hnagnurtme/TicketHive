namespace TicketHive.Application.Common.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    public IUserRepository User { get; }
    public ITokenRepository Tokens { get; }
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
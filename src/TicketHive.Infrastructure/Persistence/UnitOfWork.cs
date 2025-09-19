using TicketHive.Application.Common.Interfaces;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Infrastructure.Persistence.Repositories;

namespace TicketHive.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    private readonly Dictionary<Type, object> _repositories = new();

    public ITokenRepository Tokens { get; }

    public IUserRepository User { get; }

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        Tokens = new TokenRepository(_dbContext);
        User = new UserRepository(_dbContext);
    }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        if (_repositories.TryGetValue(typeof(TEntity), out var repo))
        {
            return (IGenericRepository<TEntity>)repo;
        }

        var newRepo = new GenericRepository<TEntity>(_dbContext);
        _repositories[typeof(TEntity)] = newRepo;
        return newRepo;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}

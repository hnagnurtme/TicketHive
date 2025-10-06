using Microsoft.EntityFrameworkCore.Storage;
using TicketHive.Application.Common.Interfaces;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Infrastructure.Persistence.Repositories;

namespace TicketHive.Infrastructure.Persistence;


public class UnitOfWork : IUnitOfWork 
{
    private readonly AppDbContext _dbContext;
    private readonly Dictionary<Type, object> _repositories = new();
    private IDbContextTransaction? _currentTransaction; 

    public IOrderRepository Orders { get; }

    public ITokenRepository Tokens { get; }
    public IUserRepository Users { get; }
    public IEventRepository Events { get; }
    public ITicketRepository Tickets { get; }

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        Tokens = new TokenRepository(_dbContext);
        Users = new UserRepository(_dbContext);
        Events = new EventRepository(_dbContext);
        Tickets = new TicketRepository(_dbContext);
        Orders = new OrderRepository(_dbContext); 
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

    // ----------------------------------------------------------------------
    // TRIỂN KHAI TRANSACTION
    // ----------------------------------------------------------------------

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction is not null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }
        _currentTransaction = await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction is null)
        {
            throw new InvalidOperationException("No transaction is currently in progress to commit.");
        }
        try
        {
            await _currentTransaction.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction is null) return;
        
        try
        {
            await _currentTransaction.RollbackAsync();
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
    
    // ----------------------------------------------------------------------
    
    public void Dispose()
    {
        _dbContext.Dispose();
        _currentTransaction?.Dispose();
    }
}
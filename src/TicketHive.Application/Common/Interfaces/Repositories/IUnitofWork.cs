using TicketHive.Application.Common.Interfaces.Repositories;

namespace TicketHive.Application.Common.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    public IUserRepository Users { get; }
    public ITokenRepository Tokens { get; }
    public IEventRepository Events { get; }
    public IOrderRepository Orders { get; }
    public ITicketRepository Tickets { get; }

    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;

    // === Quản lý Giao dịch (Transaction Methods) ===
    
    /// <summary>
    /// Bắt đầu một giao dịch cơ sở dữ liệu mới.
    /// Cần thiết để đảm bảo tính nhất quán (Atomicity) cho các nghiệp vụ phức tạp.
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Lưu các thay đổi và xác nhận (Commit) giao dịch hiện tại.
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    /// Hủy bỏ (Rollback) mọi thay đổi được thực hiện trong giao dịch hiện tại.
    /// </summary>
    Task RollbackTransactionAsync();

    // === Lưu thay đổi (Đã có) ===
    
    /// <summary>
    /// Lưu tất cả các thay đổi được theo dõi vào cơ sở dữ liệu.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
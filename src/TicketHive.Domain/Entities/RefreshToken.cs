using TicketHive.Domain.Exceptions;

namespace TicketHive.Domain.Entities;

public class RefreshToken
{
    // Keys
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }

    // Token info
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }

    // Metadata
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? RevokedAt { get; private set; }
    public bool Used { get; private set; } = false;

    // Rotation
    public Guid? ReplacedByTokenId { get; private set; }
    public RefreshToken? ReplacedByToken { get; private set; }

    // Client info
    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }
    public string? DeviceFingerprint { get; private set; }

    // Navigation
    public User User { get; private set; } = null!;

    // EF Core needs empty constructor
    private RefreshToken() { }

    public RefreshToken(
        Guid userId,
        string tokenHash,
        DateTime expiresAt,
        string? ip = null,
        string? userAgent = null,
        string? deviceFingerprint = null)
    {
        UserId = userId;
        TokenHash = tokenHash ?? throw new ArgumentNullException(nameof(tokenHash));
        ExpiresAt = expiresAt;
        IpAddress = ip;
        UserAgent = userAgent;
        DeviceFingerprint = deviceFingerprint;
    }

    // Business logic
    public bool IsActive => RevokedAt == null && !Used && ExpiresAt > DateTime.UtcNow;

    public void MarkAsUsed()
    {
        if (Used) throw new TokenAlreadyUsedException("Token is already used.");
        Used = true;
    }

    public void Revoke()
    {
        if (RevokedAt != null) throw new TokenAlreadyRevokedException("Token already revoked.");
        RevokedAt = DateTime.UtcNow;
    }

    public void Replace(Guid newTokenId)
    {
        Revoke();
        ReplacedByTokenId = newTokenId;
    }
}

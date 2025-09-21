using System;
using TicketHive.Domain.Common.Events;
using TicketHive.Domain.Events;

namespace TicketHive.Domain.Entities;
/**
*  @sql schema
*  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
*  email VARCHAR(255) NOT NULL UNIQUE,
*  password_hash TEXT NOT NULL,
*  full_name VARCHAR(255) NOT NULL,
*  phone_number VARCHAR(20),
*  role VARCHAR(50) DEFAULT 'USER' CHECK (role IN ('USER', 'ADMIN', 'ORGANIZER')),
*  email_verified BOOLEAN DEFAULT FALSE,
*  is_active BOOLEAN DEFAULT TRUE,
*  last_login_at TIMESTAMP,
*  login_attempts INT DEFAULT 0,
*  locked_until TIMESTAMP,
*  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
*  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
**/
public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string FullName { get; private set; }
    public string? PhoneNumber { get; private set; }
    public UserRole Roles { get; private set; } = UserRole.USER;
    public bool EmailVerified { get; private set; } = false;
    public bool IsActive { get; private set; } = true;
    public DateTime? LastLoginAt { get; private set; }
    public int LoginAttempts { get; private set; } = 0;
    public DateTime? LockedUntil { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();

    private User()
    {
        Email = string.Empty;
        PasswordHash = string.Empty;
        FullName = string.Empty;
    }
    public User? Organizer { get; private set; }

    public User(string email, string passwordHash, string fullName, string? phoneNumber, UserRole roles = UserRole.USER)
    {
        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        FullName = fullName;
        PhoneNumber = phoneNumber;
        AddDomainEvent(new UserRegisteredDomainEvent(Id, email));
    }
    public void UpdateLogin(DateTime loginTime)
    {
        LastLoginAt = loginTime;
        LoginAttempts = 0;
        LockedUntil = null;
        Touch();

    }

    public void IncrementLoginAttempts()
    {
        LoginAttempts++;
        Touch();
    }

    public void LockAccount(DateTime until)
    {
        LockedUntil = until;
        Touch();
    }

    public void UpdateProfile(string fullName, string? phoneNumber)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Touch();
    }
    public void MarkEmailAsVerified()
    {
        EmailVerified = true;
        Touch();
    }

    public bool IsLocked => LockedUntil.HasValue && LockedUntil > DateTime.UtcNow;

    private void Touch() => UpdatedAt = DateTime.UtcNow;

    public void AddRole(UserRole role)
    {
        Roles |= role; 
        Touch();
    }

    public void RemoveRole(UserRole role)
    {
        Roles &= ~role; 
        Touch();
    }

    public bool HasRole(UserRole role) => Roles.HasFlag(role);

    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}

[Flags]
public enum UserRole
{
    NONE  = 0,
    USER = 1 << 0,
    ADMIN = 1 << 1,
    ORGANIZER = 1 << 2

}


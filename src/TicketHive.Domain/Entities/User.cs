using System;

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
    public string PhoneNumber { get; private set; }
    public string Role { get; private set; } = "USER";
    public bool EmailVerified { get; private set; } = false;
    public bool IsActive { get; private set; } = true;
    public DateTime? LastLoginAt { get; private set; }
    public int LoginAttempts { get; private set; } = 0;
    public DateTime? LockedUntil { get; private set; }
    public DateTime? CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; } = DateTime.UtcNow;

    public User(string email, string passwordHash, string fullName, string phoneNumber)
    {
        Email = email;
        PasswordHash = passwordHash;
        FullName = fullName;
        PhoneNumber = phoneNumber;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateLogin(DateTime loginTime)
    {
        LastLoginAt = loginTime;
        LoginAttempts = 0;
        LockedUntil = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncrementLoginAttempts()
    {
        LoginAttempts++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void LockAccount(DateTime until)
    {
        LockedUntil = until;
        UpdatedAt = DateTime.UtcNow;
    }
}
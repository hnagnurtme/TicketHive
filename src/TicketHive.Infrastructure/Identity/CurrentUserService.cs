using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TicketHive.Application.Common.Interfaces;

namespace TicketHive.Infrastructure.Identity;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId =>
        Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
            ? id
            : Guid.Empty;

    public string? Email =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public string? FullName =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

    public string? PhoneNumber =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.MobilePhone);

    public string? Role =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
}

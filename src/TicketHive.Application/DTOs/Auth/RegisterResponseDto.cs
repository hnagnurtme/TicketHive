using TicketHive.Application.DTOs.Users;

namespace TicketHive.Application.DTOs.Auth;

public record RegisterResponseDto(string Token, UserDTO User);
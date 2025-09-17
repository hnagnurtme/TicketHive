using TicketHive.Application.DTOs.Users;

namespace TicketHive.Application.DTOs.Auth;

public record LoginResponseDto(string Token, UserDTO User);



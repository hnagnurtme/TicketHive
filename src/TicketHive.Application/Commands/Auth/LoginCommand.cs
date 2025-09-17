using MediatR;
using TicketHive.Application.DTOs.Auth;

namespace TicketHive.Application.Commands.Auth;

public record LoginCommand (string Email, string Password) : IRequest<LoginResponseDto>;

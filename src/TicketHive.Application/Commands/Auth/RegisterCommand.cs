using MediatR;
using TicketHive.Application.DTOs.Users;
namespace TicketHive.Application.Commands.Auth;

public record RegisterCommand (string Email, string Password, string FullName, string PhoneNumber) : IRequest<UserDTO>;
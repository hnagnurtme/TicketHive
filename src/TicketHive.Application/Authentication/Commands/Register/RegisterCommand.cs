using ErrorOr;
using MediatR;

namespace TicketHive.Application.Authentication;

public record RegisterCommand (string Email, string Password, string FullName, string PhoneNumber) : IRequest<ErrorOr<AuthenticationResult>>;
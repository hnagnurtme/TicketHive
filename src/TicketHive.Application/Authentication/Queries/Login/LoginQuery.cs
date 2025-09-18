using ErrorOr;
using MediatR;


namespace TicketHive.Application.Authentication;

public record LoginQuery (string Email, string Password) : IRequest<ErrorOr<AuthenticationResult>>;

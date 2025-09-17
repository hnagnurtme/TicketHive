using ErrorOr;
using MediatR;
using TicketHive.Application.Authentication;


namespace TicketHive.Application.Queries.Auth;

public record LoginQuery (string Email, string Password) : IRequest<ErrorOr<AuthenticationResult>>;

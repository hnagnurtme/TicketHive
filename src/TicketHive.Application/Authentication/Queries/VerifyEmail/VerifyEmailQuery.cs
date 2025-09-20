using MediatR;
using ErrorOr;

namespace TicketHive.Application.Authentication;

public record VerifyEmailQuery(string Email, string Token) : IRequest<ErrorOr<AuthenticationResult>>;
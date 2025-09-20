using MediatR;
using ErrorOr;
using TicketHive.Application.Users.Result;

namespace TicketHive.Application.Users.Query;

public record GetUserProfileQuery(string UserId) : IRequest<ErrorOr<UserProfileResult>>;
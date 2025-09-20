using MediatR;
using ErrorOr;

namespace TicketHive.Application.Users.Query;

public record GetUserProfileQuery(string UserId) : IRequest<ErrorOr<UserProfileResult>>;
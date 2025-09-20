using ErrorOr;
using MediatR;
using TicketHive.Application.Users.Result;

namespace TicketHive.Application.Users.Command;

public record class UpdateUserProfileCommand : IRequest<ErrorOr<UpdatedUserProfileResult>>
{
    public string UserId { get; init; } = null!;
    public string? FullName { get; init; }
    public string? PhoneNumber { get; init; }
}

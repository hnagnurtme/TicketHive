using MediatR;
using ErrorOr;
using TicketHive.Application.Users.Result;
using TicketHive.Application.Common.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Common.Exceptions;
namespace TicketHive.Application.Users.Command;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, ErrorOr<UpdatedUserProfileResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<UpdateUserProfileCommandHandler> _logger;

    public UpdateUserProfileCommandHandler(IUnitOfWork unitOfWork,ILogger<UpdateUserProfileCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<ErrorOr<UpdatedUserProfileResult>> Handle(
    UpdateUserProfileCommand request,
    CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.UserId, out var userId))
        {
            _logger.LogWarning("Invalid user id format: {UserId}", request.UserId);
            throw new InvalidFormatException("Invalid user id format.");
        }

        var user = await _unitOfWork.User.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            _logger.LogWarning("User not found: {UserId}", request.UserId);
            throw new NotFoundException("User not found.");
        }

        if (request.FullName != null || request.PhoneNumber != null)
        {
            user.UpdateProfile(
                request.FullName ?? user.FullName,
                request.PhoneNumber ?? user.PhoneNumber
            );

            _unitOfWork.User.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        var result = new UpdatedUserProfileResult(
            user.Id.ToString(),
            user.Email,
            user.FullName,
            user.PhoneNumber ?? string.Empty
        );

        return result;
    }

}
namespace TicketHive.Application.Users.Query;
using MediatR;
using ErrorOr;
using TicketHive.Application.Common.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Users.Result;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, ErrorOr<UserProfileResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<GetUserProfileQueryHandler> _logger;

    public GetUserProfileQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUserProfileQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<UserProfileResult>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.UserId, out var userId))
        {
            _logger.LogWarning("Invalid user id format: {UserId}", request.UserId);
            throw new ArgumentException("Invalid user id format.");
        }

        var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            _logger.LogWarning("User not found: {UserId}", request.UserId);
            throw new KeyNotFoundException("User not found.");
        }

        var result = new UserProfileResult(
            user.Email,
            user.FullName,
            user.PhoneNumber ?? string.Empty
        );

        return result;
    }
}
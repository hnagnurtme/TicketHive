namespace TicketHive.Application.Events.Command;

using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Common.Constants;
using TicketHive.Application.Common.Exceptions;
using TicketHive.Application.Common.Interfaces;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Application.Exceptions;
using TicketHive.Domain.Entities;

public class AddEventCommandHandler : IRequestHandler<AddEventCommand, ErrorOr<AddEventResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService currentUserService;
    
    private readonly ILogger<AddEventCommandHandler> _logger;

    public AddEventCommandHandler(IUnitOfWork unitOfWork, ILogger<AddEventCommandHandler> logger, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        this.currentUserService = currentUserService;
    }
    public async Task<ErrorOr<AddEventResult>> Handle(AddEventCommand request, CancellationToken cancellationToken)
    {
        // Extract user info from currentUserService
        var userId = currentUserService.UserId;
        var userEmail = currentUserService.Email;
        var userFullName = currentUserService.FullName;
        var userPhoneNumber = currentUserService.PhoneNumber;
        var userRole = currentUserService.Roles;
        // validate 
        ValidateUserInfo(userId, userEmail, userFullName, userPhoneNumber);
        ValidateUserPermissions(userRole);

        // Map request to domain event entity
        var newEvent = new Event(
                name: request.Name,
                slug: request.Slug,
                location: request.Location,
                startTime: request.StartTime,
                endTime: request.EndTime,
                organizerId: userId,
                description: request.Description,
                imageUrl: request.ImageUrl,
                venueCapacity: request.VenueCapacity,
                saleStartTime: request.SaleStartTime,
                saleEndTime: request.SaleEndTime,
                isFeatured: request.IsFeatured
            );

        await _unitOfWork.Events.AddAsync(newEvent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        AddEventResult addEventResult = new AddEventResult(
            newEvent.Id,
            newEvent.Name,
            newEvent.Slug,
            newEvent.Description,
            newEvent.Location,
            newEvent.StartTime,
            newEvent.EndTime,
            newEvent.VenueCapacity,
            newEvent.SaleStartTime,
            newEvent.SaleEndTime,
            newEvent.ImageUrl,
            newEvent.IsFeatured,
            newEvent.OrganizerId,
            newEvent.Status,
            newEvent.CreatedAt
        );
        return addEventResult;
    }

    private void ValidateUserInfo(Guid userId, string? email, string? fullName, string? phoneNumber)
    {
        if (userId == Guid.Empty || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(phoneNumber))
        {
            _logger.LogWarning("Invalid user information: UserId={UserId}, Email={Email}, FullName={FullName}, PhoneNumber={PhoneNumber}", userId, email, fullName, phoneNumber);
            throw new UnAuthorizationException("User information is incomplete or invalid.");
        }
    }
    
    private void ValidateUserPermissions(List<string> roles)
    {
        if (roles == null || !roles.Contains(RoleConstants.ORGANIZER))
        {
            _logger.LogWarning("User does not have the required permissions. Roles: {Roles}", roles);
            throw new UnAuthorizationException("You do not have permission to create an event.");
        }
    }
}
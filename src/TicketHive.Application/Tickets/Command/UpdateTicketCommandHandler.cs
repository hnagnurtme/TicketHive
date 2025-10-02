using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Common.Interfaces;
using TicketHive.Application.Common.Interfaces.Repositories;

namespace TicketHive.Application.Tickets;

public class UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand, ErrorOr<TicketDetailResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateTicketCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTicketCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateTicketCommandHandler> logger, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<TicketDetailResult>> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        if (currentUserId == Guid.Empty)
        {
            _logger.LogWarning("Invalid user ID from current user service");
            return Error.Unauthorized("User.Unauthorized", "User not authorized");
        }

        var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.TicketId, cancellationToken);

        if (ticket == null)
        {
            _logger.LogInformation("Ticket not found with ID: {TicketId}", request.TicketId);
            return Error.NotFound("Ticket.NotFound", "Ticket not found");
        }

        _logger.LogInformation("Updating ticket with ID: {TicketId} by user: {UserId}", request.TicketId, currentUserId);

        ticket.UpdatePrice(request.Price, currentUserId);
        ticket.ChangeQuantity(request.TotalQuantity, currentUserId);
        ticket.UpdateSalePeriod(request.SaleStartTime, request.SaleEndTime, currentUserId);
        ticket.UpdateGeneralInfo(request.Name, request.Description, request.SortOrder, currentUserId);

        if (!request.IsActive && ticket.IsActive)
        {
            ticket.Deactivate(currentUserId);
        }

        _unitOfWork.Tickets.Update(ticket);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully updated ticket with ID: {TicketId}", request.TicketId);

        var remainingQuantity = ticket.Inventory?.RemainingQuantity ?? 0;

        return new TicketDetailResult(
            ticket.Id,
            ticket.EventId,
            ticket.Type,
            ticket.Name,
            ticket.Price,
            ticket.TotalQuantity,
            remainingQuantity,
            ticket.MinPurchase,
            ticket.MaxPurchase,
            ticket.Description,
            ticket.OriginalPrice,
            ticket.SaleStartTime,
            ticket.SaleEndTime,
            ticket.IsActive,
            ticket.SortOrder,
            ticket.CreatedAt,
            ticket.UpdatedAt,
            ticket.CreatedBy,
            ticket.UpdatedBy
        );
    }
}
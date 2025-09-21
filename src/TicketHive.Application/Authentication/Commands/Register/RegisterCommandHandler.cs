using MediatR;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Domain.Entities;
using TicketHive.Application.Common.Interfaces;
using ErrorOr;
using TicketHive.Domain.Exceptions;
using System.Security.Claims;
using TicketHive.Application.Common.Interfaces.Events;
using Microsoft.Extensions.Logging;

namespace TicketHive.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IHashService _hashService;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IDomainEventDispatcher _domainEventDispatcher;

        private readonly ILogger<RegisterCommandHandler> _logger;

        public RegisterCommandHandler(
            IHashService hashService,
            ILogger<RegisterCommandHandler> logger,
            IDomainEventDispatcher domainEventDispatcher,
            IUnitOfWork unitOfWork)
        {
            _hashService = hashService;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Users.ExistsByEmailAsync(request.Email))
            {
                _logger.LogWarning("Email {Email} is already in use.", request.Email);
                throw new DuplicateEmailException("Email is already in use.");
            }

            var passwordHash = _hashService.Hash(request.Password);
            var user = new User(request.Email, passwordHash, request.FullName, request.PhoneNumber);

            await _unitOfWork.Users.AddAsync(user, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName ?? string.Empty),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            await _domainEventDispatcher.DispatchEventsAsync(user.DomainEvents);
            user.ClearDomainEvents();

            var userDto = new UserDTO(
                user.Id,
                user.Email,
                user.FullName ?? string.Empty,
                user.PhoneNumber ?? string.Empty,
                user.EmailVerified,
                user.CreatedAt,
                user.UpdatedAt
            );

            return new AuthenticationResult(null, null, userDto);
        }
    }
}

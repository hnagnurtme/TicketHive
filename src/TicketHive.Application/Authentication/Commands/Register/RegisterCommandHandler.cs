using MediatR;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Domain.Entities;
using TicketHive.Application.Common.Interfaces;
using ErrorOr;
using TicketHive.Domain.Exceptions;
using System.Security.Claims;

namespace TicketHive.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IHashService _hashService;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(
            IHashService hashService,
            IJwtService jwtService,
            IUnitOfWork unitOfWork)
        {
            _hashService = hashService;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.User.ExistsByEmailAsync(request.Email))
            {
                throw new DuplicateEmailException();
            }

            var passwordHash = _hashService.Hash(request.Password);
            var user = new User(request.Email, passwordHash, request.FullName, request.PhoneNumber);

            await _unitOfWork.User.AddAsync(user, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName ?? string.Empty),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var accessToken = _jwtService.GenerateToken(claims);

            var userDto = new UserDTO(
                user.Id,
                user.Email,
                user.FullName ?? string.Empty,
                user.PhoneNumber ?? string.Empty,
                user.CreatedAt,
                user.UpdatedAt
            );

            return new AuthenticationResult(accessToken, null, userDto);
        }
    }
}

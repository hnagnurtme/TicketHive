using MediatR;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Domain.Entities;
using TicketHive.Domain.Exceptions.Base;
using TicketHive.Application.Common.Interfaces;
using TicketHive.Application.Authentication;
using ErrorOr;

namespace TicketHive.Application.Handlers.Auth
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        public RegisterCommandHandler(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.ExistsByEmailAsync(request.Email))
                throw new DomainException("Email already exists", "Email already exists");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, 10);
            var user = new User(request.Email, passwordHash, request.FullName, request.PhoneNumber);

            await _userRepository.CreateUserAsync(user);


            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.FullName ),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.MobilePhone, user.PhoneNumber),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, user.Role)
            };

            var token = _jwtService.GenerateToken(claims);
            var userDto = new UserDTO(user.Id, user.Email, user.FullName, user.PhoneNumber, user.CreatedAt ?? DateTime.UtcNow, user.UpdatedAt ?? DateTime.UtcNow);
            
            return new AuthenticationResult(token, userDto);
        }
    }
}

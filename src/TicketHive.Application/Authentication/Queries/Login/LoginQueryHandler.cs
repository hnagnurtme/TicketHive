namespace TicketHive.Application.Authentication;

using MediatR;
using TicketHive.Application.Common.Interfaces.Repositories;
using Domain.Exceptions.Base;
using TicketHive.Application.Common.Interfaces;
using Domain.Entities;
using BCrypt.Net;
using System.Security.Claims;
using ErrorOr;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        public LoginQueryHandler(IUserRepository userRepository, IJwtService jwtService)
        {
                _userRepository = userRepository;
                _jwtService = jwtService;
        }
        public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
                var user = await GetUserByEmailAsync(request.Email);
                if (user == null)
                {
                        throw new DomainException("User not found", "User not found");
                }

                if (!VerifyPassword(request.Password, user.PasswordHash))
                {
                        throw new DomainException("Invalid password", "Invalid password");
                }

                await UpdateUserLoginAsync(user);

                var claims = new List<Claim>
                {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, user.FullName ?? string.Empty),
                        new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
                        new Claim(ClaimTypes.Role, user.Role)
                };
                var token = _jwtService.GenerateToken(claims);
                var userDto = new UserDTO(user.Id, user.Email, user.FullName ?? string.Empty, user.PhoneNumber ?? string.Empty, user.CreatedAt ?? DateTime.UtcNow, user.UpdatedAt ?? DateTime.UtcNow);
                return new AuthenticationResult(token, userDto);
        }

        private async Task<User?> GetUserByEmailAsync(string email)
        {
                return await _userRepository.GetUserByEmailAsync(email);
        }
        private bool VerifyPassword(string password, string passwordHash)
        {
                return BCrypt.Verify(password, passwordHash);
        }
        private async Task UpdateUserLoginAsync(User user)
        {
                user.UpdateLogin(DateTime.UtcNow);
                await _userRepository.UpdateUserAsync(user);
        }
}
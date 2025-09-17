namespace TicketHive.Application.Handlers.Auth;

using MediatR;
using DTOs.Users;
using Interfaces.Repositories;
using Domain.Exceptions.Base;
using Interfaces;
using TicketHive.Application.Commands.Auth;
using TicketHive.Application.DTOs.Auth;
using Domain.Entities;
using BCrypt.Net;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        public LoginCommandHandler(IUserRepository userRepository, IJwtService jwtService)
        {
                _userRepository = userRepository;
                _jwtService = jwtService;
        }
        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
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

                var token = _jwtService.GenerateToken(user);
                var userDto = new UserDTO(user.Id, user.Email, user.FullName, user.PhoneNumber, user.Role);
                return new LoginResponseDto(token, userDto);
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
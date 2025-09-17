using System.Threading;
using System.Threading.Tasks;
using MediatR;
using BCrypt.Net;
using TicketHive.Application.Commands.Auth;
using TicketHive.Application.DTOs.Users;
using TicketHive.Application.Interfaces.Repositories;
using TicketHive.Domain.Entities;
using TicketHive.Domain.Exceptions.Base;
using TicketHive.Application.Interfaces;

namespace TicketHive.Application.Handlers.Auth
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        public RegisterCommandHandler(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<UserDTO> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if(await _userRepository.ExistsByEmailAsync(request.Email))
                throw new DomainException("Email already exists", "Email already exists");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, 10);
            var user = new User(request.Email, passwordHash, request.FullName, request.PhoneNumber);

            await _userRepository.CreateUserAsync(user);


            var token = _jwtService.GenerateToken(user);
            return new UserDTO(user.Id, user.Email, user.FullName, user.PhoneNumber, user.Role, token);
        }
    }
}

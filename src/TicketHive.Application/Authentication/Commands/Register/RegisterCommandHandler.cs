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
        private readonly IUserRepository _userRepository;
        private readonly IHashService _hashService;

        public RegisterCommandHandler(IUserRepository userRepository, IHashService hashService)
        {
            _userRepository = userRepository;
            _hashService = hashService;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.ExistsByEmailAsync(request.Email))
            {
                throw new DuplicateEmailException();
            }
            var passwordHash = _hashService.Hash(request.Password);

            var user = new User(request.Email, passwordHash, request.FullName, request.PhoneNumber);

            await _userRepository.CreateUserAsync(user);


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName ),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? ""),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var userDto = new UserDTO(user.Id, user.Email, user.FullName, user.PhoneNumber ?? "", user.CreatedAt , user.UpdatedAt);

            return new AuthenticationResult(null, null, userDto);
        }
    }
}

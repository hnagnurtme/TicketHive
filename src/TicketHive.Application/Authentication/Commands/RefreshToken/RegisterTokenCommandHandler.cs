using MediatR;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Application.Common.Interfaces;
using ErrorOr;

namespace TicketHive.Application.Authentication.Commands.RefreshToken;

public class GenerateRefreshTokenCommandHandler 
    : IRequestHandler<GenerateRefreshTokenCommand, ErrorOr<RefreshTokenResult>>
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public GenerateRefreshTokenCommandHandler(
        ITokenRepository tokenRepository, 
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _tokenRepository = tokenRepository;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<ErrorOr<RefreshTokenResult>> Handle(
        GenerateRefreshTokenCommand request, 
        CancellationToken cancellationToken)
    {
        var refreshToken = _refreshTokenGenerator.Generate(
            request.UserId,
            request.IpAddress,
            request.UserAgent,
            request.DeviceFingerprint,
            out var plainToken);

        await _tokenRepository.AddAsync(refreshToken, cancellationToken);

        return new RefreshTokenResult(plainToken, refreshToken.ExpiresAt);
    }
}

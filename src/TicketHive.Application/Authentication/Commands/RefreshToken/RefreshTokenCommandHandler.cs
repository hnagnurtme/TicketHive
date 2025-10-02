using MediatR;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Application.Common.Interfaces;
using ErrorOr;
using TicketHive.Domain.Entities;

namespace TicketHive.Application.Authentication.Commands.RefreshToken;

public class GenerateRefreshTokenCommandHandler 
    : IRequestHandler<GenerateRefreshTokenCommand, ErrorOr<RefreshTokenResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public GenerateRefreshTokenCommandHandler(
        IUnitOfWork unitOfWork,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _unitOfWork = unitOfWork;
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

        await _unitOfWork.Tokens.AddAsync(refreshToken, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RefreshTokenResult(plainToken, refreshToken.ExpiresAt);
    }
}

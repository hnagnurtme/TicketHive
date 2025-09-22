using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using TicketHive.Application.Authentication;
using TicketHive.Application.Authentication.Commands.RefreshToken;
using TicketHive.Application.Common.Interfaces;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Domain.Entities;
using Xunit;

namespace TicketHive.Tests.ApplicationTests.Authentication
{
    public class GenerateRefreshTokenCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ITokenRepository> _tokenRepoMock;
        private readonly Mock<IRefreshTokenGenerator> _refreshTokenGeneratorMock;
        private readonly GenerateRefreshTokenCommandHandler _handler;

        public GenerateRefreshTokenCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _tokenRepoMock = new Mock<ITokenRepository>();
            _refreshTokenGeneratorMock = new Mock<IRefreshTokenGenerator>();

            _unitOfWorkMock.Setup(u => u.Tokens).Returns(_tokenRepoMock.Object);

            _handler = new GenerateRefreshTokenCommandHandler(
                _unitOfWorkMock.Object,
                _refreshTokenGeneratorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldGenerateAndSaveRefreshToken_WhenRequestIsValid()
        {
            // Arrange
            var command = new GenerateRefreshTokenCommand(
                Guid.NewGuid(),
                "127.0.0.1",
                "Test User Agent",
                "test-fingerprint"
            );

            var plainToken = "my-secret-plain-text-token";
            var expiresAt = DateTime.UtcNow.AddDays(7);
            var refreshTokenEntity = new RefreshToken(
                command.UserId,
                "hashed-token",
                expiresAt,
                command.IpAddress,
                command.UserAgent,
                command.DeviceFingerprint
            );

            // We need to use a more flexible It.IsAny<string>() for the out parameter.
            // Then we can assign the desired value to it inside the callback.
            _refreshTokenGeneratorMock
                .Setup(g => g.Generate(
                    command.UserId,
                    command.IpAddress,
                    command.UserAgent,
                    command.DeviceFingerprint,
                    out It.Ref<string>.IsAny))
                .Callback(new GenerateTokenCallback((Guid userId, string ip, string ua, string fp, out string pt) => { pt = plainToken; }))
                .Returns(refreshTokenEntity);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(plainToken, result.Value.Token);
            Assert.Equal(expiresAt, result.Value.ExpiresAt);

            _tokenRepoMock.Verify(r => r.AddAsync(refreshTokenEntity, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // Helper delegate to handle the 'out' parameter in the Moq setup
        private delegate void GenerateTokenCallback(Guid userId, string ip, string ua, string fp, out string plainToken);
    }
}
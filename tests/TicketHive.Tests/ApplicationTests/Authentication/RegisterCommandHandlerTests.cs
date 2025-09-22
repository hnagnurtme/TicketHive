using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using TicketHive.Application.Authentication;
using TicketHive.Application.Authentication.Commands.Register;
using TicketHive.Application.Common.Interfaces;
using TicketHive.Application.Common.Interfaces.Events;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Domain.Entities;
using TicketHive.Domain.Exceptions;
using TicketHive.Domain.Common.Events;
using Xunit;

namespace TicketHive.Tests.ApplicationTests.Authentication
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<IHashService> _hashServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IDomainEventDispatcher> _domainEventDispatcherMock;
        private readonly Mock<ILogger<RegisterCommandHandler>> _loggerMock;
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            _hashServiceMock = new Mock<IHashService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepoMock = new Mock<IUserRepository>();
            _domainEventDispatcherMock = new Mock<IDomainEventDispatcher>();
            _loggerMock = new Mock<ILogger<RegisterCommandHandler>>();

            _unitOfWorkMock.Setup(u => u.Users).Returns(_userRepoMock.Object);

            _handler = new RegisterCommandHandler(
                _hashServiceMock.Object,
                _loggerMock.Object,
                _domainEventDispatcherMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldRegisterUser_WhenEmailIsUnique()
        {
            // Arrange
            var command = new RegisterCommand(
                "test@example.com",
                "password123",
                "Test User",
                "1234567890"
            );

            _userRepoMock.Setup(r => r.ExistsByEmailAsync(command.Email, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _hashServiceMock.Setup(h => h.Hash(command.Password)).Returns("hashed_password");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.NotNull(result.Value.User);
            Assert.Equal(command.Email, result.Value.User.Email);
            Assert.Equal(command.FullName, result.Value.User.FullName);
            Assert.Null(result.Value.AccessToken); // No token should be generated on registration
            Assert.Null(result.Value.RefreshToken);

            _userRepoMock.Verify(r => r.AddAsync(It.Is<User>(u => u.Email == command.Email && u.PasswordHash == "hashed_password"), It.IsAny<CancellationToken>()), Times.Once);
            _domainEventDispatcherMock.Verify(d => d.DispatchEventsAsync(It.IsAny<IReadOnlyCollection<IDomainEvent>>(), It.IsAny<CancellationToken>()), Times.Once);
            _domainEventDispatcherMock.Verify(d => d.DispatchEventsAsync(It.IsAny<IReadOnlyCollection<IDomainEvent>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowDuplicateEmailException_WhenEmailAlreadyExists()
        {
            // Arrange
            var command = new RegisterCommand(
                "existing@example.com",
                "password123",
                "Existing User",
                "0987654321"
            );

            _userRepoMock.Setup(r => r.ExistsByEmailAsync(command.Email, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DuplicateEmailException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Email is already in use.", exception.Message);

            // Verify that no user was added and no changes were saved
            _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
            _domainEventDispatcherMock.Verify(d => d.DispatchEventsAsync(It.IsAny<IReadOnlyCollection<IDomainEvent>>(), It.IsAny<CancellationToken>()), Times.Never);
            _domainEventDispatcherMock.Verify(d => d.DispatchEventsAsync(It.IsAny<IReadOnlyCollection<IDomainEvent>>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
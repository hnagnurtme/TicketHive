using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using TicketHive.Application.Users.Command;
using TicketHive.Application.Users.Result;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Application.Common.Exceptions;
using TicketHive.Domain.Entities;

namespace TicketHive.Tests.ApplicationTests.Users
{
    public class UpdateUserProfileCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<ILogger<UpdateUserProfileCommandHandler>> _loggerMock;
        private readonly UpdateUserProfileCommandHandler _handler;

        public UpdateUserProfileCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepoMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<UpdateUserProfileCommandHandler>>();

            _unitOfWorkMock.Setup(u => u.User).Returns(_userRepoMock.Object);
            _handler = new UpdateUserProfileCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidFormatException_WhenUserIdIsInvalid()
        {
            // Arrange
            var command = new UpdateUserProfileCommand
            {
                UserId = "not-a-guid",
                FullName = "New Name"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidFormatException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenUserNotFound()
        {
            // Arrange
            var command = new UpdateUserProfileCommand
            {
                UserId = Guid.NewGuid().ToString(),
                FullName = "New Name"
            };

            _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldUpdateFullNameAndPhoneNumber_WhenValidRequest()
        {
            // Arrange
            var user = new User(
                email: "test@example.com",
                passwordHash: "hashedpwd",
                fullName: "Old Name",
                phoneNumber: "111111"
            );

            _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var command = new UpdateUserProfileCommand
            {
                UserId = user.Id.ToString(),
                FullName = "New Name",
                PhoneNumber = "999999"
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal("New Name", result.Value.FullName);
            Assert.Equal("999999", result.Value.PhoneNumber);

            _userRepoMock.Verify(r => r.Update(It.IsAny<User>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldUpdateOnlyFullName_WhenPhoneNumberIsNull()
        {
            // Arrange
            var user = new User(
                email: "test@example.com",
                passwordHash: "hashedpwd",
                fullName: "Old Name",
                phoneNumber: "111111"
            );

            _userRepoMock.Setup(r => r.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var command = new UpdateUserProfileCommand
            {
                UserId = user.Id.ToString(),
                FullName = "New Name",
                PhoneNumber = null
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal("New Name", result.Value.FullName);
            Assert.Equal("111111", result.Value.PhoneNumber); // giữ nguyên
        }

        [Fact]
        public async Task Handle_ShouldUpdateOnlyPhoneNumber_WhenFullNameIsNull()
        {
            // Arrange
            var user = new User(
                email: "test@example.com",
                passwordHash: "hashedpwd",
                fullName: "Old Name",
                phoneNumber: "111111"
            );

            _userRepoMock.Setup(r => r.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var command = new UpdateUserProfileCommand
            {
                UserId = user.Id.ToString(),
                FullName = null,
                PhoneNumber = "999999"
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal("Old Name", result.Value.FullName); // giữ nguyên
            Assert.Equal("999999", result.Value.PhoneNumber);
        }

        [Fact]
        public async Task Handle_ShouldNotUpdateAnything_WhenBothFullNameAndPhoneNumberAreNull()
        {
            // Arrange
            var user = new User(
                email: "test@example.com",
                passwordHash: "hashedpwd",
                fullName: "Old Name",
                phoneNumber: "111111"
            );

            _userRepoMock.Setup(r => r.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var command = new UpdateUserProfileCommand
            {
                UserId = user.Id.ToString(),
                FullName = null,
                PhoneNumber = null
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal("Old Name", result.Value.FullName);
            Assert.Equal("111111", result.Value.PhoneNumber);

            _userRepoMock.Verify(r => r.Update(It.IsAny<User>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}

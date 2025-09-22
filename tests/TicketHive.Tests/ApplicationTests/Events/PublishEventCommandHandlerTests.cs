using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Moq;
using TicketHive.Application.Common.Constants;
using TicketHive.Application.Common.Exceptions;
using TicketHive.Application.Common.Interfaces;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Application.Events.Command;
using TicketHive.Application.Exceptions;
using TicketHive.Domain.Entities;
using Xunit;

namespace TicketHive.Tests.ApplicationTests.Events
{
    public class PublishEventCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEventRepository> _eventRepoMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<ILogger<PushlishEventCommandHandler>> _loggerMock;
        private readonly PushlishEventCommandHandler _handler;

        public PublishEventCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _eventRepoMock = new Mock<IEventRepository>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _loggerMock = new Mock<ILogger<PushlishEventCommandHandler>>();

            _unitOfWorkMock.Setup(u => u.Events).Returns(_eventRepoMock.Object);

            _handler = new PushlishEventCommandHandler(
                _unitOfWorkMock.Object,
                _loggerMock.Object,
                _currentUserServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldPublishEvent_WhenUserIsOrganizerAndEventIsDraft()
        {
            // Arrange
            var organizerId = Guid.NewGuid();
            var eventId = Guid.NewGuid();
            var command = new PushlishEventCommand(eventId);

            _currentUserServiceMock.Setup(s => s.UserId).Returns(organizerId);
            _currentUserServiceMock.Setup(s => s.Roles).Returns(new List<string> { RoleConstants.ORGANIZER });

            var eventEntity = new Event("Test Event", "test-event", "Test Location", DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2), organizerId);

            _eventRepoMock.Setup(r => r.GetByIdAsync(eventId, It.IsAny<CancellationToken>())).ReturnsAsync(eventEntity);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(EventStatus.PUBLISHED, result.Value.Status);
            _eventRepoMock.Verify(r => r.Update(It.Is<Event>(e => e.Status == EventStatus.PUBLISHED)), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowUnAuthorizationException_WhenUserIsNotOrganizerRole()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new PushlishEventCommand(Guid.NewGuid());

            _currentUserServiceMock.Setup(s => s.UserId).Returns(userId);
            _currentUserServiceMock.Setup(s => s.Roles).Returns(new List<string> { RoleConstants.USER });

            // Act & Assert
            await Assert.ThrowsAsync<UnAuthorizationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenEventDoesNotExist()
        {
            // Arrange
            var organizerId = Guid.NewGuid();
            var eventId = Guid.NewGuid();
            var command = new PushlishEventCommand(eventId);

            _currentUserServiceMock.Setup(s => s.UserId).Returns(organizerId);
            _currentUserServiceMock.Setup(s => s.Roles).Returns(new List<string> { RoleConstants.ORGANIZER });

            _eventRepoMock.Setup(r => r.GetByIdAsync(eventId, It.IsAny<CancellationToken>())).ReturnsAsync((Event?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidPermissionException_WhenUserIsNotEventOrganizer()
        {
            // Arrange
            var requestingUserId = Guid.NewGuid();
            var actualOrganizerId = Guid.NewGuid();
            var eventId = Guid.NewGuid();
            var command = new PushlishEventCommand(eventId);

            _currentUserServiceMock.Setup(s => s.UserId).Returns(requestingUserId);
            _currentUserServiceMock.Setup(s => s.Roles).Returns(new List<string> { RoleConstants.ORGANIZER });

            var eventEntity = new Event("Test Event", "test-event", "Test Location", DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2), actualOrganizerId);

            _eventRepoMock.Setup(r => r.GetByIdAsync(eventId, It.IsAny<CancellationToken>())).ReturnsAsync(eventEntity);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidPermissionException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenEventIsNotInDraftStatus()
        {
            // Arrange
            var organizerId = Guid.NewGuid();
            var eventId = Guid.NewGuid();
            var command = new PushlishEventCommand(eventId);

            _currentUserServiceMock.Setup(s => s.UserId).Returns(organizerId);
            _currentUserServiceMock.Setup(s => s.Roles).Returns(new List<string> { RoleConstants.ORGANIZER });

            var eventEntity = new Event("Test Event", "test-event", "Test Location", DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2), organizerId);
            eventEntity.Publish(); // Status is now PUBLISHED

            _eventRepoMock.Setup(r => r.GetByIdAsync(eventId, It.IsAny<CancellationToken>())).ReturnsAsync(eventEntity);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Only events in DRAFT status can be published.", ex.Message);
        }

        [Fact]
        public async Task Handle_ShouldThrowUnAuthorizationException_WhenUserIdIsEmpty()
        {
            // Arrange
            var command = new PushlishEventCommand(Guid.NewGuid());

            _currentUserServiceMock.Setup(s => s.UserId).Returns(Guid.Empty);
            _currentUserServiceMock.Setup(s => s.Roles).Returns(new List<string> { RoleConstants.ORGANIZER });

            // Act & Assert
            await Assert.ThrowsAsync<UnAuthorizationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}

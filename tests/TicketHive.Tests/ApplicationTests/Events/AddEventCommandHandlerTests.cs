using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using TicketHive.Application.Common.Constants;
using TicketHive.Application.Common.Exceptions;
using TicketHive.Application.Common.Interfaces;
using TicketHive.Application.Common.Interfaces.Repositories;
using TicketHive.Application.Events.Command;
using TicketHive.Domain.Entities;
using Xunit;

namespace TicketHive.Tests.ApplicationTests.Events
{
    public class AddEventCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEventRepository> _eventRepoMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<ILogger<AddEventCommandHandler>> _loggerMock;
        private readonly AddEventCommandHandler _handler;

        public AddEventCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _eventRepoMock = new Mock<IEventRepository>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _loggerMock = new Mock<ILogger<AddEventCommandHandler>>();

            _unitOfWorkMock.Setup(u => u.Events).Returns(_eventRepoMock.Object);

            _handler = new AddEventCommandHandler(
                _unitOfWorkMock.Object,
                _loggerMock.Object,
                _currentUserServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldAddEvent_WhenUserIsOrganizerAndRequestIsValid()
        {
            // Arrange
            var organizerId = Guid.NewGuid();
            _currentUserServiceMock.Setup(s => s.UserId).Returns(organizerId);
            _currentUserServiceMock.Setup(s => s.Email).Returns("organizer@test.com");
            _currentUserServiceMock.Setup(s => s.FullName).Returns("Organizer Name");
            _currentUserServiceMock.Setup(s => s.PhoneNumber).Returns("1234567890");
            _currentUserServiceMock.Setup(s => s.Roles).Returns(new List<string> { RoleConstants.ORGANIZER });

            var command = new AddEventCommand(
                "Test Event",
                "test-event",
                "A great event",
                "Test Location",
                DateTime.UtcNow.AddDays(10),
                DateTime.UtcNow.AddDays(11),
                100,
                DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(9),
                "http://example.com/image.jpg",
                false,
                organizerId
            );

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(command.Name, result.Value.Name);
            Assert.Equal(command.Slug, result.Value.Slug);
            Assert.Equal(organizerId, result.Value.OrganizerId);
            Assert.Equal(EventStatus.DRAFT, result.Value.Status);

            _eventRepoMock.Verify(r => r.AddAsync(It.Is<Event>(e => e.Name == command.Name), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowUnAuthorizationException_WhenUserIsNotOrganizer()
        {
            // Arrange
            _currentUserServiceMock.Setup(s => s.UserId).Returns(Guid.NewGuid());
            _currentUserServiceMock.Setup(s => s.Email).Returns("user@test.com");
            _currentUserServiceMock.Setup(s => s.FullName).Returns("Test User");
            _currentUserServiceMock.Setup(s => s.PhoneNumber).Returns("1234567890");
            _currentUserServiceMock.Setup(s => s.Roles).Returns(new List<string> { RoleConstants.USER });

            var command = new AddEventCommand("Test", "test", null, "loc", DateTime.Now, DateTime.Now.AddDays(1), null, null, null, null, false, Guid.NewGuid());

            // Act & Assert
            await Assert.ThrowsAsync<UnAuthorizationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", "test@test.com", "name", "123456")] // Empty Guid
        [InlineData("c1b2a3d4-e5f6-a7b8-c9d0-e1f2a3b4c5d6", "", "name", "123456")] // Empty Email
        [InlineData("c1b2a3d4-e5f6-a7b8-c9d0-e1f2a3b4c5d6", "test@test.com", "", "123456")] // Empty FullName
        [InlineData("c1b2a3d4-e5f6-a7b8-c9d0-e1f2a3b4c5d6", "test@test.com", "name", "")] // Empty PhoneNumber
        public async Task Handle_ShouldThrowUnAuthorizationException_WhenUserInfoIsIncomplete(string userId, string email, string fullName, string phoneNumber)
        {
            // Arrange
            _currentUserServiceMock.Setup(s => s.UserId).Returns(Guid.Parse(userId));
            _currentUserServiceMock.Setup(s => s.Email).Returns(email);
            _currentUserServiceMock.Setup(s => s.FullName).Returns(fullName);
            _currentUserServiceMock.Setup(s => s.PhoneNumber).Returns(phoneNumber);
            _currentUserServiceMock.Setup(s => s.Roles).Returns(new List<string> { RoleConstants.ORGANIZER });

            var command = new AddEventCommand("Test", "test", null, "loc", DateTime.Now, DateTime.Now.AddDays(1), null, null, null, null, false, Guid.NewGuid());

            // Act & Assert
            var ex = await Assert.ThrowsAsync<UnAuthorizationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("User information is incomplete or invalid.", ex.Message);
        }

        [Fact]
        public async Task Handle_ShouldThrowDomainException_WhenEventConstructorValidationFails()
        {
            // Arrange
            var organizerId = Guid.NewGuid();
            _currentUserServiceMock.Setup(s => s.UserId).Returns(organizerId);
            _currentUserServiceMock.Setup(s => s.Email).Returns("organizer@test.com");
            _currentUserServiceMock.Setup(s => s.FullName).Returns("Organizer Name");
            _currentUserServiceMock.Setup(s => s.PhoneNumber).Returns("1234567890");
            _currentUserServiceMock.Setup(s => s.Roles).Returns(new List<string> { RoleConstants.ORGANIZER });

            var command = new AddEventCommand(
                "Test Event",
                "test-event",
                "A great event",
                "Test Location",
                DateTime.UtcNow.AddDays(11), // StartTime after EndTime
                DateTime.UtcNow.AddDays(10),
                100, null, null, null, false, organizerId
            );

            // Act & Assert
            await Assert.ThrowsAsync<TicketHive.Domain.Exceptions.InValidDateException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
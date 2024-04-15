using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos;
using Prime.Progreso.Domain.Dtos.ActivityDtos;
using Prime.Progreso.Domain.Notifications.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prime.Progreso.Services.Test
{
    public class NotificationServiceTests
    {
        public Mock<IHubContext<NotificationHub, INotificationHubClient>> _hubContext;
        public INotificationService _service;
        public Notification nullNotification = null;
        public Notification notification = new Notification() { Message = "message"};

        public NotificationServiceTests()
        {
            _hubContext = new Mock<IHubContext<NotificationHub, INotificationHubClient>>();
            _service = new NotificationService(_hubContext.Object);
        }

        [Fact]
        public async Task SendNotificationToAll_NotificationIsNull_ExpectedArgumentNullException()
        {
            // Act
            async Task test() => await _service.SendNotificationToAll(nullNotification);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(test);
        }

        [Fact]
        public async Task SendNotificationToUser_NotificationIsNull_ExpectedArgumentNullException()
        {
            // Act
            async Task test() => await _service.SendNotificationToUserAsync(nullNotification, It.IsAny<string>());

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(test);
        }

        [Fact]
        public async Task SendNotificationToGroup_NotificationIsNull_ExpectedArgumentNullException()
        {
            // Act
            async Task test() => await _service.SendNotificationToGroupAsync(nullNotification, It.IsAny<string>());

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(test);
        }
        [Fact]
        public async Task AddUserToGroup_ConnectionIdIsNull_ExpectedArgumentNullException()
        {
            // Act
            async Task test() => await _service.SendNotificationToGroupAsync(nullNotification, It.IsAny<string>());

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(test);
        }
    }
}

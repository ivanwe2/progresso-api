using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos;
using Prime.Progreso.Domain.Notifications.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub, INotificationHubClient> _hubContext;

        public NotificationService(IHubContext<NotificationHub, INotificationHubClient> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task SendNotificationToAll(Notification notification)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }
            await _hubContext.Clients.All.ReceiveNotification(notification);
        }

        public async Task SendNotificationToGroupAsync(Notification notification, string groupName)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }
            await _hubContext.Clients.Group(groupName).ReceiveNotification(notification);
        }

        public async Task SendNotificationToUserAsync(Notification notification, string user)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }
            await _hubContext.Clients.Client(user).ReceiveNotification(notification);
        }

        public async Task AddUserToGroup(string connectionId, string user)
        {
            if (connectionId is null)
            {
                throw new ArgumentNullException(nameof(connectionId));
            }
            await _hubContext.Groups.AddToGroupAsync(connectionId, user);
        }
    }
}

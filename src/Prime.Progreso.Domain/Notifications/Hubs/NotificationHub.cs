using Microsoft.AspNetCore.SignalR;
using Prime.Progreso.Domain.Dtos;

namespace Prime.Progreso.Domain.Notifications.Hubs
{
    public class NotificationHub : Hub<INotificationHubClient>
    {

        public async Task SendNotificationAsync(string user, Notification notification)
            => await Clients.User(user).ReceiveNotification(notification);


        public async Task SendNotificationToGroupAsync(string group, Notification notification)
            => await Clients.Group(group).ReceiveNotification(notification);


        public async Task AddUserToGroupAsync(string groupName = "HubUsers")
            => await Groups.AddToGroupAsync(Context.ConnectionId, groupName);


        public async Task RemoveUserFromGroupAsync(string groupName = "HubUsers")
            => await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);


        public override async Task OnConnectedAsync()
        {
            await AddUserToGroupAsync();
            await Clients.All.ReceiveNotification(new Notification { Message = $"New client login: {Context.ConnectionId}, {Context.UserIdentifier}" });
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public Task ThrowException(Exception exception)
             => throw new HubException(exception.Message);
    }
}

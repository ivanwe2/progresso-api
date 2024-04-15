using Prime.Progreso.Domain.Dtos;

namespace Prime.Progreso.Domain.Notifications.Hubs
{
    public interface INotificationHubClient
    {
        Task ReceiveNotification(Notification notification);
    }
}

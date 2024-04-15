using Prime.Progreso.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.Abstractions.Services
{
    public interface INotificationService
    {
        Task SendNotificationToAll(Notification notification);
        Task SendNotificationToGroupAsync(Notification notification, string groupName);
        Task SendNotificationToUserAsync(Notification notification, string user);
        Task AddUserToGroup(string connectionId, string user);
    }
}

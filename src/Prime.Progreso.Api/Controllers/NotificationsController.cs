using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prime.Progreso.Api.Constants;
using Prime.Progreso.Domain.Abstractions.Services;
using Prime.Progreso.Domain.Dtos;

namespace Prime.Progreso.Api.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    [Authorize(Policy = PolicyConstants.ApiKeyPolicy)]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        [Route("notify-all")]
        public async Task<IActionResult> SendNotificationToAllAsync([FromBody] Notification notification)
        {
            await _notificationService.SendNotificationToAll(notification);
            return Ok(notification);
        }

        [HttpPost]
        [Route("notify-user")]
        public async Task<IActionResult> SendNotificationToUserAsync([FromBody] Notification notification,string userId)
        {
            await _notificationService.SendNotificationToUserAsync(notification,userId);
            return Ok(notification);
        }

        [HttpPost]
        [Route("notify-group")]
        public async Task<IActionResult> SendNotificationToGroupAsync([FromBody] Notification notification,string groupName)
        {
            await _notificationService.SendNotificationToGroupAsync(notification,groupName);
            return Ok(notification);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Services;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using turbin.sikker.core.Model.DTO.NotificationDtos;

namespace turbin.sikker.core.Controllers
{
    [Authorize(Policy = "AuthZPolicy")]
    [EnableCors("AllowAllHeaders")]
    [ApiController]
    [Route("api")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        private readonly INotificationUtilities _notificationUtilities;

        public NotificationController(INotificationService notificationService, INotificationUtilities notificationUtilities)
        {
            _notificationService = notificationService;
            _notificationUtilities = notificationUtilities;
        }

        [HttpGet("GetAllNotifications")]
        [SwaggerOperation(Summary = "Get all notifications", Description = "Retrieves a list of all notifications")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<NotificationResponseDto>))]
        [SwaggerResponse(404, "Notifications not found")]
        public async Task<IActionResult> GetAllNotificationsAsync()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();

            if (notifications == null)
            {
                return NotFound("Notifications not found");
            }

            return Ok(notifications);
        }

        [HttpGet("GetNotificationByUserId")]
        [SwaggerOperation(Summary = "Get all notifications for a user", Description = "Retrieves a list of invoices for a specfic user")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<NotificationResponseDto>))]
        [SwaggerResponse(404, "Notifications not found")]
        public async Task<IActionResult> GetNotificationsByUserIdAsync(string id)
        {
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(id);
            return Ok(notifications);
        }

        [HttpPost("AddNotification")]
        [SwaggerOperation(Summary = "Create new notification", Description = "Creates a new notification")]
        [SwaggerResponse(201, "Notification created")]
        public async Task<IActionResult> CreateNotificationAsync(NotificationCreateDto notification)
        {
            await _notificationService.CreateNotificationAsync(notification);

            return Ok();
        }

        [HttpPost("UpdateNotififcation")]
        [SwaggerOperation(Summary = "Update notification", Description = "Updtaes a notification")]
        [SwaggerResponse(200, "Notification updates")]
        public async Task UpdateNotificationAsync(NotificationUpdateDto notification)
        {
            await _notificationService.UpdateNotificationAsync(notification);
        }
    }
}

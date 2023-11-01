using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Services;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using turbin.sikker.core.Model.DTO.NotificationDtos;
using Azure.Messaging.WebPubSub;

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

        [HttpGet("GetPubSubAccessToken")]
        [SwaggerOperation(Summary = "Get an access token for pubsub", Description = "Retrieves an access token for pubsub")]
        [SwaggerResponse(200, "Success", typeof(string))]
        public async Task<IActionResult> GetAccessToken()
        {
            var client = new WebPubSubServiceClient(Environment.GetEnvironmentVariable("pubSubConnectionString"), "Hub");
            var uri = await client.GetClientAccessUriAsync(new TimeSpan(24, 0, 0));
            var token = new { token = uri };
            return Ok(token);
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

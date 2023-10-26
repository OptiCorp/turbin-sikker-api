using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Services;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

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
    }
}

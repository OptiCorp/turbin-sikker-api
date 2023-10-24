using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Utilities
{
    public class NotificationUtilities : INotificationUtilities
    {
        public bool IsValidStatus(string value)
        {
            string lowerCaseValue = value.ToLower();
            return lowerCaseValue == "Active" || lowerCaseValue == "Inactive";
        }

        public string GetNotificationStatus(NotificationStatus status)
        {
            switch (status)
            {
                case NotificationStatus.Active:
                    return "Active";
                case NotificationStatus.Inactive:
                    return "Inactive";
                default:
                    return "";
            }
        }

        public NotificationResponseDto NotificationToResponseDto(Notification? notification, byte[]? bytes)
        {
            if (bytes == null)
            {
                return new NotificationResponseDto
                {
                    Id = notification.Id,
                    Message = notification.Message,
                    // NotificationStatus = GetNotificationStatus(notification.NotificationStatus),
                    CreatedDate = notification.CreatedDate,
                    UpdatedDate = notification.UpdatedDate,
                    NotificationType = notification.NotificationType,
                };
            }
            return new NotificationResponseDto
            {
                Id = notification.Id,
                Message = notification.Message,
                // NotificationStatus = GetNotificationStatus(notification.NotificationStatus),
                CreatedDate = notification.CreatedDate,
                UpdatedDate = notification.UpdatedDate,
                NotificationType = notification.NotificationType,
            };
        }
    }
}

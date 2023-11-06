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
                case NotificationStatus.Unread:
                    return "Unread";
                case NotificationStatus.Read:
                    return "Read";
                default:
                    return "";
            }
        }

        public string GetNotificationType(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Error:
                    return "Error";
                case NotificationType.Warning:
                    return "Warning";
                case NotificationType.Info:
                    return "Info";
                default:
                    return "";
            }
        }

        public NotificationResponseDto NotificationToResponseDto(Notification? notification)
        {
            return new NotificationResponseDto
            {
                Id = notification.Id,
                Message = notification.Message,
                NotificationStatus = GetNotificationStatus(notification.NotificationStatus),
                CreatedDate = notification.CreatedDate,
                UpdatedDate = notification.UpdatedDate,
                NotificationType = GetNotificationType(notification.NotificationType),
                ReceiverId = notification.ReceiverId
            };
        }
    }
}

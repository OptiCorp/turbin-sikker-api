using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;


namespace turbin.sikker.core.Utilities
{
    public interface INotificationUtilities
    {
        bool IsValidStatus(string value);
        string GetNotificationStatus(NotificationStatus status);

        public NotificationResponseDto NotificationToResponseDto(Notification? notification);
    }
}

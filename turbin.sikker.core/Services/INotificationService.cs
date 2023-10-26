using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public interface INotificationService
    {
        // Task<IEnumerable<NotificationResponseDto>> GetAllNotificationsAsync();
        Task<IEnumerable<NotificationResponseDto>> GetAllNotificationsAsync();
        Task AddNotification(string errorMessage);
        // Task<NotificationResponseDto> GetNotificationByIdAsync(string id);
        // Task<NotificationResponseDto> GetNotificationPdfByNotificationIdAsync(string id);
        // Task CreateNotificationAsync(NotificationCreateDto notification);
        // Task UpdateNotificationAsync(NotificationUpdateDto notification);
        // Task DeleteNotificationAsync(string id);
    }
}

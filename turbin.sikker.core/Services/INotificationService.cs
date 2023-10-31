using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Model.DTO.NotificationDtos;

namespace turbin.sikker.core.Services
{
    public interface INotificationService
    {
        // Task<IEnumerable<NotificationResponseDto>> GetAllNotificationsAsync();
        Task<IEnumerable<NotificationResponseDto>> GetAllNotificationsAsync();
        Task<IEnumerable<NotificationResponseDto>> GetNotificationsByUserIdAsync(string id);
        Task CreateNotificationAsync(NotificationCreateDto notification);
        Task UpdateNotificationAsync(NotificationUpdateDto updatedNotification);
        // Task<NotificationResponseDto> GetNotificationByIdAsync(string id);
        // Task<NotificationResponseDto> GetNotificationPdfByNotificationIdAsync(string id);
        // Task CreateNotificationAsync(NotificationCreateDto notification);
        // Task UpdateNotificationAsync(NotificationUpdateDto notification);
        // Task DeleteNotificationAsync(string id);
    }
}

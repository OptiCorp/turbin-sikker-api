using System.Text.Json;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Model.DTO.NotificationDtos;
using turbin.sikker.core.Utilities;

namespace turbin.sikker.core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly TurbinSikkerDbContext _context;

        private readonly INotificationUtilities _notificationUtilities;
        private readonly IChecklistService _checklistService;

        public NotificationService(TurbinSikkerDbContext context, INotificationUtilities notificationUtilities, IChecklistService checklistService)
        {
            _context = context;
            _notificationUtilities = notificationUtilities;
            _checklistService = checklistService;
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetAllNotificationsAsync()
        {
            return await _context.Notification
                            .OrderByDescending(c => c.CreatedDate)
                            .Select(p => _notificationUtilities.NotificationToResponseDto(p))
                            .ToListAsync();
        }

        public async Task CreateNotificationAsync(NotificationCreateDto notification)
        {
            Notification newNotification = new Notification
            {
                Message = notification.Message,
                NotificationStatus = NotificationStatus.Active, // TODO: change to read and unread
                CreatedDate = DateTime.Now,
                NotificationType = Enum.Parse<NotificationType>(notification.NotificationType)
            };

            await _context.Notification.AddAsync(newNotification);
            await _context.SaveChangesAsync();
        }
    }
}

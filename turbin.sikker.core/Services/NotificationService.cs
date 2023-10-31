using System.Text.Json;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.WebPubSub;
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
                            .OrderBy(c => c.NotificationStatus)
                            .ThenByDescending(c => c.CreatedDate)
                            .Select(p => _notificationUtilities.NotificationToResponseDto(p))
                            .ToListAsync();
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetNotificationsByUserIdAsync(string id)
        {
            return await _context.Notification.Where(c => c.ReceiverId == id).Select(n => _notificationUtilities.NotificationToResponseDto(n)).ToListAsync();
        }

        public async Task CreateNotificationAsync(NotificationCreateDto notification)
        {
            Notification newNotification = new Notification
            {
                Message = notification.Message,
                NotificationStatus = NotificationStatus.Unread, // TODO: change to read and unread
                CreatedDate = DateTime.Now,
                NotificationType = Enum.Parse<NotificationType>(notification.NotificationType),
                ReceiverId = notification.ReceiverId
            };

            await _context.Notification.AddAsync(newNotification);
            await _context.SaveChangesAsync();

            var serviceClient = new WebPubSubServiceClient("Endpoint=https://pub-sub-test.webpubsub.azure.com;AccessKey=QZWtGVoO7OFHum7s53t5ZiukRagDDbtHR9s+DP7WvkI=;Version=1.0;", "hub");
            await serviceClient.SendToAllAsync("hello");
        }

        public async Task UpdateNotificationAsync(NotificationUpdateDto updatedNotification)
        {
            var notification = await _context.Notification.FirstOrDefaultAsync(n => n.Id == updatedNotification.Id);
            if (notification == null)
            {
                return;
            }
            notification.NotificationStatus = Enum.Parse<NotificationStatus>(updatedNotification.NotificationStatus);
            await _context.SaveChangesAsync();
        }
    }
}

using System.Text.Json;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;
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
                            .Select(p => _notificationUtilities.NotificationToResponseDto(p, null))
                            .ToListAsync();
        }

        public async Task AddNotification(string errorMessage)
        {
            Notification notification = new Notification
            {
                Message = errorMessage,
                NotificationStatus = NotificationStatus.Active, // TODO: change to read and unread
                CreatedDate = DateTime.Now,
                NotificationType = NotificationType.Error
            };

            await _context.Notification.AddAsync(notification);
            await _context.SaveChangesAsync();

        }
    }
}

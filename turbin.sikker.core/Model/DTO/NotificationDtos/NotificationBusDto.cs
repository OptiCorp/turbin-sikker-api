using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class NotificationBusDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public NotificationStatus NotificationStatus { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime UpdatedDate { get; set; }
        [Required]
        public NotificationType NotificationType { get; set; }
    }
}

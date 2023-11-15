using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class NotificationBusDto
    {
        [Required]
        public string Message { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        [Required]
        public string NotificationType { get; set; }
    }
}

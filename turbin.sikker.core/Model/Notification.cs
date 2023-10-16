using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace turbin.sikker.core.Model
{
    public enum NotificationStatus
    {
        [Display(Name = "Active")]
        Active,
        [Display(Name = "Inactive")]
        Inactive
    }
    public enum NotificationType
    {
        [Display(Name = "Error")]
        Error,
        [Display(Name = "Info")]
        Info,
        [Display(Name = "Warning")]
        Warning
    }

    public class Notification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Message { get; set; }
        [EnumDataType(typeof(NotificationStatus))]
        public NotificationStatus NotificationStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [EnumDataType(typeof(NotificationType))]
        public NotificationType NotificationType { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace turbin.sikker.core.Model
{
    public enum NotificationStatus
    {
        [Display(Name = "Active")]
        Paid,
        [Display(Name = "Inactive")]
        Unpaid
    }

    public class Notification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Title { get; set; }
        [EnumDataType(typeof(NotificationStatus))]
        public NotificationStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime SentDate { get; set; }
    }
}

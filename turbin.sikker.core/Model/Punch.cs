using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace turbin.sikker.core.Model
{
    public enum PunchSeverity
    {
        [Display(Name = "Minor")]
        Minor,
        [Display(Name = "Major")]
        Major,
        [Display(Name = "Critical")]
        Critical
    }
    public enum PunchStatus
    {
        [Display(Name = "Pending")]
        Pending,
        [Display(Name = "Approved")]
        Approved,
        [Display(Name = "Rejected")]
        Rejected
    }

    public class Punch
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ChecklistId { get; set; }

        //[Required]
        //[StringLength(450)]
        //public string UserId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(450)]
        public string CreatedBy { get; set; }

        public User? CreatedByUser { get; }

        [Required]
        [StringLength(1500)]
        public string PunchDescription { get; set; }


        // Enum? ('Minor', 'Major', 'Critical')
        //public int Severity { get; set; }


        [EnumDataType(typeof(PunchSeverity))]
        public PunchSeverity Severity { get; set; }

        [EnumDataType(typeof(PunchStatus))]
        public PunchStatus Status { get; set; }

        // Boolean? 
        public Byte Active { get; set; }
    }
}


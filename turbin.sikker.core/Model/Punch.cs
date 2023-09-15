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
        public string? ChecklistWorkflowId { get; set; }
        
        public ChecklistWorkflow? ChecklistWorkflow { get; }

        [Required]
        public string? ChecklistTaskId {get; set; }

        public ChecklistTask? ChecklistTask { get; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [Required]
        [StringLength(450)]
        public string? CreatedBy { get; set; }

        public User? CreatedByUser { get; }

        [Required]
        [StringLength(1500)]
        public string? PunchDescription { get; set; }

        [Required]
        [EnumDataType(typeof(PunchSeverity))]
        public PunchSeverity Severity { get; set; }

        [Required]
        [EnumDataType(typeof(PunchStatus))]
        public PunchStatus Status { get; set; }

        // Boolean? 
        public Byte Active { get; set; }

        public ICollection<Upload>? Uploads { get; }
    }
}


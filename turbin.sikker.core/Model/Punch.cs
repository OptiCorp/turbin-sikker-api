﻿using System.ComponentModel.DataAnnotations;
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
        public string? WorkflowId { get; set; }
        
        public Workflow? Workflow { get; }

        [Required]
        public string? ChecklistTaskId {get; set; }

        public ChecklistTask? ChecklistTask { get; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [Required]
        [StringLength(450)]
        public string? CreatorId { get; set; }

        public User? Creator { get; }

        // [Required]
        // [StringLength(1500)]
        public string? Description { get; set; }

        [Required]
        [EnumDataType(typeof(PunchSeverity))]
        public PunchSeverity Severity { get; set; }

        [Required]
        [EnumDataType(typeof(PunchStatus))]
        public PunchStatus Status { get; set; }

        // Boolean? 
        public Byte Active { get; set; }

        public ICollection<Upload>? Uploads { get; }

        public string? Message { get; set; }
    }
}


﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace turbin.sikker.core.Model
{
    public enum WorkflowStatus
    {
        [Display(Name = "Sent")]
        Sent,
        [Display(Name = "Committed")]
        Committed,
        [Display(Name = "Done")]
        Done,
        [Display(Name = "Rejected")]
        Rejected
    }

    public class Workflow
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        [Required]
        public string? ChecklistId { get; set; }

        public Checklist? Checklist { get; }

        [Required]
        public string? UserId { get; set; }

        public User? User { get; }

        [Required]
        public string? CreatorId { get; set; }

        public User? Creator { get; }

        [EnumDataType(typeof(WorkflowStatus))]
        public WorkflowStatus? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? CompletionTimeMinutes { get; set; }

        public string? InvoiceId { get; set; }

        public ICollection<TaskInfo>? TaskInfos { get; }

        public string? Comment { get; set; }

    }
}


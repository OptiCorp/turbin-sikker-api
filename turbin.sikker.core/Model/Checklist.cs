﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace turbin.sikker.core.Model
{
    public enum ChecklistStatus
    {
        [Display(Name = "Active")]
        Active,
        [Display(Name = "Inactive")]
        Inactive
    }

    public class Checklist
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Title { get; set; }

        [Required]
        [EnumDataType(typeof(ChecklistStatus))]
        public ChecklistStatus Status { get; set; }
        
        [Required]
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [Required]
        public string? CreatedBy { get; set; }

        public User? CreatedByUser { get; }
        public ICollection<ChecklistTask>? ChecklistTasks { get; }

    }
}


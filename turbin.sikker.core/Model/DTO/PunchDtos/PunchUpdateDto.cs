﻿using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class PunchUpdateDto
    {
        public string? PunchDescription { get; set; }

        public string? ChecklistWorkflowId { get; set; }

        public string? Severity { get; set; }

        public string? Status { get; set; }

        [Required]
        public string? Id { get; set; }
    }
}


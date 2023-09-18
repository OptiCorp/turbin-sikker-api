using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class PunchCreateDto
    {   
        [Required]
        public string? CreatedBy { get; set; }

        [Required]
        public string? PunchDescription { get; set; }

        [Required]
        public string? ChecklistWorkflowId { get; set; }

        [Required]
        public string? ChecklistTaskId { get; set; }

        [Required]
        public string? Severity { get; set; }

    }
}


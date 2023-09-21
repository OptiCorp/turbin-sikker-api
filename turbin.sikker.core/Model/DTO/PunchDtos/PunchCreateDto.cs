using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class PunchCreateDto
    {   
        [Required]
        public string? CreatorId { get; set; }

        // [Required]
        public string? Description { get; set; }

        [Required]
        public string? WorkflowId { get; set; }

        [Required]
        public string? ChecklistTaskId { get; set; }

        [Required]
        public string? Severity { get; set; }

    }
}


using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class PunchUpdateDto
    {
        [Required]
        public string? Id { get; set; }
        public string? Description { get; set; }

        public string? WorkflowId { get; set; }

        public string? Severity { get; set; }

        public string? Status { get; set; }

        public string? Message { get; set; }
    }
}


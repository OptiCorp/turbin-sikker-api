using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.WorkflowDtos
{
    public class WorkflowUpdateDto
    {
        [Required]
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? Status { get; set; }
        public int? CompletionTimeMinutes { get; set; }
        public ICollection<TaskInfoUpdate>? TaskInfos { get; set; }
        public string? Comment { get; set; }

    }
}

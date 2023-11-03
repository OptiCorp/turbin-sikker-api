using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.TaskDtos
{
    public class ChecklistTaskChecklistDto
    {
        [Required]
        public string? TaskId { get; set; }

        [Required]
        public string? ChecklistId { get; set; }
    }
}

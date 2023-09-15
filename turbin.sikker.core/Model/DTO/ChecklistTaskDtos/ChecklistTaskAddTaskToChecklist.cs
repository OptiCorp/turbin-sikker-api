using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.TaskDtos
{
    public class ChecklistTaskAddTaskToChecklistDto
    {   
        [Required]
        public string? Id { get; set; }

        [Required]
        public string? ChecklistId { get; set; }
    }
}

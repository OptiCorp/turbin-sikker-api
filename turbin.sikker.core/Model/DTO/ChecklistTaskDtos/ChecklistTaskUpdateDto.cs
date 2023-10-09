using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.TaskDtos
{
    public class ChecklistTaskUpdateDto
    {   
        [Required]
        public string? Id { get; set; }
        public string? CategoryId { get; set; }

        public string? Description { get; set; }

        public string? ChecklistId { get; set; }

        public int? EstAvgCompletionTime { get; set; }
    }
}

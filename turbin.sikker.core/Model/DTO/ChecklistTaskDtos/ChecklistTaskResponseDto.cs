using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.TaskDtos
{
    public class ChecklistTaskResponseDto
    {
        [Required]
        public string? Id { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public Category? Category { get; set; }
    }
}

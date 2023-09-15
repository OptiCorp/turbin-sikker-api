using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.TaskDtos
{
    public class ChecklistTaskByCategoryResponseDto
    {
        [Required]
        public string? Id { get; set; }

        [Required]
        public string? Description { get; set; }

    }
}
using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.TaskDtos
{
    public class ChecklistTaskRequestDto
    {   
        [Required]
        public string? CategoryId { get; set; }

        [Required]
        public string? Description { get; set; }


    }
}

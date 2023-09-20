using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.TaskDtos
{
    public class ChecklistTaskCreateDto
    {   
        [Required]
        public string? CategoryId { get; set; }

        [Required]
        public string? Description { get; set; }


    }
}

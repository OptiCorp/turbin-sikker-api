using System.ComponentModel.DataAnnotations.Schema;

namespace turbin.sikker.core.Model.DTO.TaskDtos
{
    public class ChecklistTaskRequestDto
    {
        public string? CategoryId { get; set; }

        public string? Description { get; set; }


    }
}

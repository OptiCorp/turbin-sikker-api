using System.ComponentModel.DataAnnotations.Schema;

namespace turbin.sikker.core.Model.DTO.TaskDtos
{
    public class ChecklistTaskResponseDto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        public string Description { get; set; }

        public Category? Category { get; set; }
    }
}

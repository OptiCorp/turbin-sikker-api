using System.ComponentModel.DataAnnotations;
using turbin.sikker.core.Model.DTO.TaskDtos;

namespace turbin.sikker.core.Model.DTO.ChecklistDtos
{
    public class ChecklistCreateDto
    {

            [Required]
            [StringLength(50)]
            public string Title { get; set; }

            public string CreatedBy { get; set; }

            public ICollection<ChecklistTaskRequestDto>? ChecklistTasks { get; set; }
        
    }
}

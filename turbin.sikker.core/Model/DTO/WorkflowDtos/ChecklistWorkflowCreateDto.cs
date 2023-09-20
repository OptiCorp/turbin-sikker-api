using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.WorkflowDtos
{
    public class WorkflowCreateDto
    {   
        [Required]
        public string? ChecklistId { get; set; }

        [Required]
        public ICollection<string>? UserIds { get; set; }

        [Required]
        public string? CreatorId { get; set; }

    }
}
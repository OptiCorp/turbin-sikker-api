using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.ChecklistWorkflowDtos
{
    public class ChecklistWorkflowCreateDto
    {   
        [Required]
        public string? ChecklistId { get; set; }

        [Required]
        public ICollection<string>? UserIds { get; set; }

        [Required]
        public string? CreatorId { get; set; }

    }
}
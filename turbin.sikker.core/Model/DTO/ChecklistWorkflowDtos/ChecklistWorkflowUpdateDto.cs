using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.ChecklistWorkflowDtos
{
    public class ChecklistWorkflowUpdateDto
    {   
        [Required]
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? Status { get; set; }
        
    }
}

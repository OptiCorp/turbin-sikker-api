using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.ChecklistWorkflowDtos
{
    public class ChecklistWorkflowEditDto
    {   
        public string? UserId { get; set; }
        public string? Status { get; set; }
        
        [Required]
        public string? Id { get; set; }
    }
}

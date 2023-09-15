using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.ChecklistWorkflowDtos
{
    public class ChecklistWorkflowResponseDto
    {   
        [Required]
        public string? Id { get; set; }
        
        [Required]
        public Checklist? Checklist { get; set; }

        [Required]
        public User? User { get; set; }

        [Required]
        public User? Creator { get; set; }

        [Required]
        public string? Status { get; set; }

        [Required]
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

    }
}


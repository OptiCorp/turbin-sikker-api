using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace turbin.sikker.core.Model
{
    public enum CurrentChecklistStatus
    {
        [Display(Name = "Sent")]
        Sent,
        [Display(Name = "Committed")]
        Committed,
        [Display(Name = "Done")]
        Done
    }
    public class ChecklistWorkflow
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        [Required]
        public string? ChecklistId { get; set; }
        
        public Checklist? Checklist { get; }

        [Required]
        public string? UserId { get; set; }

        public User? User { get; }

        [Required]
        public string? CreatorId{ get; set; }

        public User? Creator { get; }

        [EnumDataType(typeof(CurrentChecklistStatus))]
        public CurrentChecklistStatus? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

    }
}


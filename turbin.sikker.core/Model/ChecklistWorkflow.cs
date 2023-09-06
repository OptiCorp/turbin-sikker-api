using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;

namespace turbin.sikker.core.Model
{
    public enum CurrentChecklistStatus
    {
        [Display(Name = "Sent")]
        Sent,
        [Display(Name = "Committed")]
        Commited,
        [Display(Name = "Done")]
        Done
    }
    public class ChecklistWorkflow
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        public string? ChecklistId { get; set; }
        
        public Checklist? Checklist { get; }

        public string? Title { get; }

        public string? UserId { get; set; }

        public User? User { get; }

        public string? CreatedById { get; set; }

        public User? Creator { get; }

        [EnumDataType(typeof(CurrentChecklistStatus))]
        public CurrentChecklistStatus? Status { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

    }
}


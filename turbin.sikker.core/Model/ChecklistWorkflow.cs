using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string Id { get; set; }

        public string ChecklistId { get; set; }

        public string UserId { get; set; }

        [EnumDataType(typeof(CurrentChecklistStatus))]
        public CurrentChecklistStatus Status { get; set; }

        public DateTime UpdatedDate { get; set; }

    }
}


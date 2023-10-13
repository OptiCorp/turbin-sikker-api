using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace turbin.sikker.core.Model
{
    public enum TaskInfoStatus
    {
        [Display(Name = "Unfinished")]
        Unfinished,
        [Display(Name = "Finished")]
        Finished,
        [Display(Name = "Not applicable")]
        NotApplicable
    }

    public class TaskInfo
    {   
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        public string TaskId { get; set; }
        public TaskInfoStatus Status { get; set; }
    }
}
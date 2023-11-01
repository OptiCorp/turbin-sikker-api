using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace turbin.sikker.core.Model
{
    public enum TaskInfoStatus
    {
        [Display(Name = "Unfinished")]
        Unfinished,
        [Display(Name = "Finished")]
        Finished,
        [Display(Name = "NotApplicable")]
        NotApplicable
    }

    [PrimaryKey(nameof(TaskId), nameof(WorkflowId))]
    public class TaskInfo
    {
        public string TaskId { get; set; }
        public TaskInfoStatus Status { get; set; }
        public string WorkflowId { get; set; }
    }
}

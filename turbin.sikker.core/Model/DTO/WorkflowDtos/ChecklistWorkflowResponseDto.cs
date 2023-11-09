
using turbin.sikker.core.Model.DTO.ChecklistDtos;

namespace turbin.sikker.core.Model.DTO.WorkflowDtos
{
    public class WorkflowResponseDto
    {
        public string? Id { get; set; }

        public ChecklistInWorkflowResponseDto? Checklist { get; set; }

        public User? User { get; set; }

        public User? Creator { get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? CompletionTimeMinutes { get; set; }

        public string? InvoiceId { get; set; }

        public Dictionary<string, string>? TaskInfos { get; set; }

        public string? Comment { get; set; }
    }
}


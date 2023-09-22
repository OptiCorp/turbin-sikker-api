
namespace turbin.sikker.core.Model.DTO
{
    public class PunchResponseDto
    {   
        public string? Id { get; set; }

        public string? WorkflowId { get; set; }

        public ChecklistTask? ChecklistTask { get; set; }

        public User? User { get; set; }

        public string? Status { get; set; }

        public string? Message { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Description { get; set; }

        public string? Severity { get; set; }

        public Byte Active { get; set; }

        public ICollection<Upload>? Uploads { get; set; }

    }
}


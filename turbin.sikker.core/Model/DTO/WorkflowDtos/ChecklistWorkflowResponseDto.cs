
namespace turbin.sikker.core.Model.DTO.WorkflowDtos
{
    public class WorkflowResponseDto
    {   
        public string? Id { get; set; }
        
        public Checklist? Checklist { get; set; }

        public User? User { get; set; }

        public User? Creator { get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

    }
}

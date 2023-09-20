
namespace turbin.sikker.core.Model.DTO
{
    public class PunchResponseDto
    {   
        public string? Id { get; set; }

        public ChecklistTask? ChecklistTask { get; set; }

        public User? User { get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Description { get; set; }

        public string? Severity { get; set; }

        public Byte Active { get; set; }

    }
}


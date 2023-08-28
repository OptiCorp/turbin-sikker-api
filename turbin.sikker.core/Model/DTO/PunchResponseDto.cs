namespace turbin.sikker.core.Model.DTO
{
    public class PunchResponseDto
    {
        public string? Id { get; set; }

        public string ChecklistId { get; set; }

        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string PunchDescription { get; set; }

        public string Severity { get; set; }

        public Byte Active { get; set; }

        public string CreatedBy { get; set; }

        public User? User { get; set; }

    }
}


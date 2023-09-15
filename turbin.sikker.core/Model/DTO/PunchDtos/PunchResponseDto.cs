using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class PunchResponseDto
    {   
        [Required]
        public string? Id { get; set; }

        public ChecklistTask? ChecklistTask { get; set; }

        [Required]
        public string? Status { get; set; }

        [Required]
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [Required]
        public string? PunchDescription { get; set; }

        [Required]
        public string? Severity { get; set; }

        public Byte Active { get; set; }

        public User? User { get; set; }

    }
}


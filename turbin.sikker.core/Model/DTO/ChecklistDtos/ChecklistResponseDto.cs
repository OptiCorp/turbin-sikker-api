using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.ChecklistDtos
{
    public class ChecklistResponseDto
    {   
        public string? Id { get; set; }

        [StringLength(50)]
        public string? Title { get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public User? User { get; set; }
        public ICollection<ChecklistTask>? ChecklistTasks { get; set; }

        public int? EstCompletionTimeMinutes { get; set; }
    }
}

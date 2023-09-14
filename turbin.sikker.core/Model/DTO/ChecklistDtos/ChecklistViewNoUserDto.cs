using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.ChecklistDtos
{
    public class ChecklistViewNoUserDto
    {   
        [Required]
        public string? Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Status { get; set; }

        [Required]
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public ICollection<ChecklistTask>? ChecklistTasks { get; set; }
    }
}

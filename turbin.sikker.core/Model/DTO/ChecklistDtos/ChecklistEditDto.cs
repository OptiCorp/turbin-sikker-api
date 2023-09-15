using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.ChecklistDtos
{
    public class ChecklistEditDto
    {   
        [Required]
        public string? Id { get; set; }

        [StringLength(50)]
        public string? Title { get; set; }

        public string? Status { get; set; }

    }
}

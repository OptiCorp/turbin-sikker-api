using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.ChecklistDtos
{
    public class ChecklistEditDto
    {

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public CStatus ChecklistStatus { get; set; }

    }
}

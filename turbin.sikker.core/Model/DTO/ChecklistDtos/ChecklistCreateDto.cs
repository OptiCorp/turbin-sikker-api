using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.ChecklistDtos
{
    public class ChecklistCreateDto
    {

            [Required]
            [StringLength(50)]
            public string Title { get; set; }

            public string CreatedBy { get; set; }


        
    }
}

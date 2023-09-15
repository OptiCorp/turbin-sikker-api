using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.CategoryDtos
{
    public class CategoryUpdateDto
    {
        [Required]
        public string? Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.CategoryDtos
{
    public class CategoryCreateDto
    {
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

    }
}

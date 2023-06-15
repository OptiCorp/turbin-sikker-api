using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class UserRoleCreateDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}

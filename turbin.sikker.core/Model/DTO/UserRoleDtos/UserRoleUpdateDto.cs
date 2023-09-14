using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class UserRoleUpdateDto
    {
        [StringLength(50)]
        public string Name { get; set; }
        public string Id { get; set; }
    }
}

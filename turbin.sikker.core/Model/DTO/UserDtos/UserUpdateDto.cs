using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class UserUpdateDto
    {   
        [StringLength(150)]
        public string? Username { get; set; }

        [StringLength(150)]
        public string? FirstName { get; set; }

        [StringLength(150)]
        public string? LastName { get; set; }

        [EmailAddress]
        [StringLength(300)]
        public string? Email { get; set; }

        [StringLength(150)]
        public string? UserRoleId { get; set; }

        public string? Status { get; set; }

        [Required]
        public string? Id { get; set; }
    }
}

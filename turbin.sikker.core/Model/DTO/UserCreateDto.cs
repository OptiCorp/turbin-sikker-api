using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class UserCreateDto
    {
        [Required]
        [StringLength(150)]
        public string Username { get; set; }

        [Required]
        [StringLength(150)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(150)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(150)]
        public string UserRoleId { get; set; }

        [Required]
        [StringLength(500)]
        public string Password { get; set; }
    }
}

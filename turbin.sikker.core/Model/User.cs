using System.ComponentModel.DataAnnotations;
namespace turbin.sikker.core.Model
{
    public class User
    {
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string UserRoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

    }
}

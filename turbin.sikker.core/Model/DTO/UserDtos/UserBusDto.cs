using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace turbin.sikker.core.Model
{
    public enum UserAction
    {
        [Display(Name = "Created")]
        Created,
        [Display(Name = "Updated")]
        Updated,
        [Display(Name = "SoftDeleted")]
        SoftDeleted,
        [Display(Name = "HardDeleted")]
        HardDeleted,
    }
    public class UserBusDto
    {
        public string? Id { get; set; }

        public string? AzureAdUserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Username { get; set; }

        public string? UserRole { get; set; }

        public string? Status { get; set; }

        [Required]
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Action { get; set; }
    }
}

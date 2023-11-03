using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model
{
    public class UserBusUpdateDto
    {
        public string? Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Username { get; set; }

        public string? UserRole { get; set; }

        public string? Status { get; set; }

        public DateTime? UpdatedDate { get; set; }

    }
}

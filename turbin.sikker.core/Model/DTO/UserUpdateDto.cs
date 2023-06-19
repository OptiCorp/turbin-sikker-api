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

        [StringLength(300)]
        public string? Email { get; set; }

        [StringLength(150)]
        public string? UserRoleId { get; set; }

        [StringLength(500)]
        public string? Password { get; set; }

        [EnumDataType(typeof(UserStatus))]
        public UserStatus Status { get; set; }
    }
}

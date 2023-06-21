using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO.ChecklistDtos
{
    public class ChecklistMultipleResponseDto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [EnumDataType(typeof(ChecklistStatus))]
        public ChecklistStatus Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public User? User { get; set; }
    }
}

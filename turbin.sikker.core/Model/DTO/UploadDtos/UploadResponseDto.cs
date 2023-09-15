using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class UploadResponseDto
    {   
        [Required]
        public string? Id { get; set; }
        [Required]
        public string? PunchId { get; set; }
        [Required]
        public string? BlobRef { get; set; }
    }
}


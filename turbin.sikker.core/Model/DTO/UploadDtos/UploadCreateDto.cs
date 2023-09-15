using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class UploadCreateDto
    {   
        [Required]
        public string? PunchId { get; set; }

        [Required]
        public string? BlobRef { get; set; }
    }
}


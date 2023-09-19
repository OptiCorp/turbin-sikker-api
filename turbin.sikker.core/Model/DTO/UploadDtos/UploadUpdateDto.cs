using System.ComponentModel.DataAnnotations;

namespace turbin.sikker.core.Model.DTO
{
    public class UploadUpdateDto
    {   
        [Required]
        public string? Id { get; set; }
        public string? PunchId { get; set; }
    }
}


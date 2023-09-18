
namespace turbin.sikker.core.Model.DTO
{
    public class UploadResponseDto
    {   
        public byte[] Bytes { get; set; }
        public string? Id { get; set; }
        public string? PunchId { get; set; }
        public string? BlobRef { get; set; }
    }
}


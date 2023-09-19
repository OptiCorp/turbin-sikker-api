
namespace turbin.sikker.core.Model.DTO
{
    public class UploadResponseDto
    {   
        public string? Id { get; set; }
        public string? PunchId { get; set; }
        public string? BlobRef { get; set; }
        public byte[] Bytes { get; set; }
        public string ContentType { get; set; }
    }
}


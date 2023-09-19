using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Utilities
{
public class UploadUtilities : IUploadUtilities
	{
        public UploadResponseDto ToResponseDto(Upload upload, byte[] bytes)
        {
            return new UploadResponseDto
            {
                Id = upload.Id,
                PunchId = upload.PunchId,
                BlobRef = upload.BlobRef,
                Bytes = bytes,
                ContentType = upload.ContentType
            };
        }
    }
}
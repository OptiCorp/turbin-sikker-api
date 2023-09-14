using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;


namespace turbin.sikker.core.Utilities
{
public interface IUploadUtilities
    {
        public UploadResponseDto ToResponseDto(Upload upload);
    }
}
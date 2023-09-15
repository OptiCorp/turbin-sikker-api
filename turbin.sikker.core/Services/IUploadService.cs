using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;

namespace turbin.sikker.core.Services
{
    public interface IUploadService
    {
        Task<IEnumerable<UploadResponseDto>> GetAllUploads();
        Task<UploadResponseDto> GetUploadById(string id);
        Task<IEnumerable<UploadResponseDto>> GetUploadsByPunchId(string punchId);
        Task UpdateUpload(UploadUpdateDto upload);
        Task<string> CreateUpload(UploadCreateDto upload);
        Task DeleteUpload(string id);
    }
}